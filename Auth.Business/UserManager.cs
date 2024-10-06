using Auth.Business.Interfaces;
using Auth.Common.Enums;
using Auth.Common.Helper;
using Auth.Common.Mappings;
using Auth.Common.Models.Request;
using Auth.Common.Models.Response;
using Auth.Data.UoW;
using System.Net;
using System.Transactions;

namespace Auth.Business
{
    public class UserManager : IUserManager
    {
        private readonly UserMapping _userMapping;
        private readonly IUnitOfWork _uow;
        private readonly PasswordPolicyHelper _passwordPolicyHelper;

        public UserManager(UserMapping userMapping, IUnitOfWork uow, PasswordPolicyHelper passwordPolicyHelper)
        {
            _userMapping = userMapping;
            _uow = uow;
            _passwordPolicyHelper = passwordPolicyHelper;
        }

        public async Task<BaseResponseModel> CreateUser(RegisterRequestModel registerRequestModel)
        {
            _passwordPolicyHelper.CheckPasswordPolicy(registerRequestModel.Password);

            var isExist = await _uow.UserRepository.Any(x => x.Email == registerRequestModel.Email);

            if (isExist)
            {
                throw new CustomException("Bu email kullanımda", HttpStatusCode.BadRequest);
            }

            var userEntity = _userMapping.MapToUserEntity(registerRequestModel);

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await _uow.UserRepository.Add(userEntity);
                await _uow.SaveChanges();

                var roleEntity = await _uow.RoleRepository.GetById((int)RoleType.User);
                var userRoleEntity = _userMapping.MapToUserRoleEntity(userEntity, roleEntity);

                await _uow.UserRoleRepository.Add(userRoleEntity);

                var userPasswordHistoryEntity = _userMapping.MapToUserPasswordHistoryEntity(userEntity.Id, userEntity.Password);
                await _uow.UserPasswordHistoryRepository.Add(userPasswordHistoryEntity);

                transaction.Complete();
            }

            await _uow.SaveChanges();

            return new BaseResponseModel().ToSuccessResponse("Kayıt eklendi.");
        }

        public async Task<LoginResponseModel> GetUser(LoginRequestModel loginRequestModel)
        {
            var user = await _uow.UserRepository.GetUserByEmailWithRoles(loginRequestModel.Email);

            if (user is null)
            {
                throw new CustomException("Böyle bir kullanıcı bulunamadı", HttpStatusCode.Unauthorized);
            }
            
            if (user.IsBlocked)
            {
                throw new CustomException("Hesabınız bloke olmuştur", HttpStatusCode.Unauthorized);
            }

            var hashedPassword = loginRequestModel.Password.ToHashedData();

            if (user.Password != hashedPassword)
            {
                user.PasswordAtteps++;

                if (user.PasswordAtteps == 3)
                {
                    user.IsBlocked = true;
                }

                await _uow.SaveChanges();

                throw new CustomException("Böyle bir kullanıcı bulunamadı.", HttpStatusCode.BadRequest);
            }

            user.PasswordAtteps = 0;
            await _uow.SaveChanges();

            return new LoginResponseModel
            {
                AccessToken = TokenHelper.CreateAndGetToken(user)
            }.ToSuccessResponse();
        }

        public async Task<BaseResponseModel> UpdatePassword(UpdateRequestModel updateRequestModel, string token)
        {
            _passwordPolicyHelper.CheckPasswordPolicy(updateRequestModel.NewPassword);

            var decodedToken = TokenHelper.ValidateToken(token.Replace("Bearer ", ""));
            var emailAddress = decodedToken.Claims.FirstOrDefault(x => x.Type.Contains("emailaddress"))?.Value;

            if (emailAddress is null)
            {
                throw new CustomException("Geçersiz kullanıcı", HttpStatusCode.Unauthorized);
            }

            var user = await _uow.UserRepository.GetFirst(x => x.Email == emailAddress);

            if (user is null)
            {
                throw new CustomException("Geçersiz kullanıcı", HttpStatusCode.Unauthorized);
            }

            if (updateRequestModel.CurrentPassword.ToHashedData() != user.Password)
            {
                throw new CustomException("Geçersiz kullanıcı", HttpStatusCode.Unauthorized);
            }

            var lastThreePassword = await _uow.UserPasswordHistoryRepository.GetLastThreePasswordByUserId(user.Id);

            if (lastThreePassword.Contains(updateRequestModel.NewPassword.ToHashedData()))
            {
                throw new CustomException("Şifreniz son 3 şifrenizden farklı olmalıdır", HttpStatusCode.BadRequest);
            }

            user.Password = updateRequestModel.NewPassword.ToHashedData();

            var userPasswordHistoryEntity = _userMapping.MapToUserPasswordHistoryEntity(user.Id, updateRequestModel.NewPassword.ToHashedData());

            await _uow.UserPasswordHistoryRepository.Add(userPasswordHistoryEntity);
            await _uow.SaveChanges();

            return new BaseResponseModel().ToSuccessResponse("Şifreniz güncellendi");
        }
    }
}
