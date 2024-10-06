using Auth.Common.Enums;
using Auth.Common.Helper;
using Auth.Common.Models.Request;
using Auth.Data.Entities;

namespace Auth.Common.Mappings
{
    public class UserMapping
    {
        public User MapToUserEntity(RegisterRequestModel registerRequestModel)
        {
            return new User
            {
                Name = registerRequestModel.Name,
                Surname = registerRequestModel.Surname,
                Email = registerRequestModel.Email,
                Password = registerRequestModel.Password.ToHashedData(),
                BirthDate = registerRequestModel.BirthDate,
                Gender = registerRequestModel.Gender == GenderType.Female ? 0 : 1,
                CreatedDate = DateTime.Now,
                IsActive = true
            };
        }

        public UserRole MapToUserRoleEntity(User user, Role role)
        {
            return new UserRole
            {
                Role = role,
                User = user,
                UserId = user.Id,
                RoleId = role.Id,
            };
        }

        public UserPasswordHistory MapToUserPasswordHistoryEntity(int userId, string password)
        {
            return new UserPasswordHistory
            {
                Password = password,
                UserId = userId,
                CreatedDate = DateTime.Now
            };
        }
    }
}
