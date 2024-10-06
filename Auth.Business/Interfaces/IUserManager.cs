using Auth.Common.Models.Request;
using Auth.Common.Models.Response;

namespace Auth.Business.Interfaces
{
    public interface IUserManager
    {
        Task<BaseResponseModel> CreateUser(RegisterRequestModel registerRequestModel);
        Task<LoginResponseModel> GetUser(LoginRequestModel loginRequestModel);
        Task<BaseResponseModel> UpdatePassword(UpdateRequestModel updateRequestModel, string token);
    }
}
