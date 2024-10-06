using Auth.Business.Interfaces;
using Auth.Common.Helper;
using Auth.Common.Models.Request;
using Auth.Common.Models.Response;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserManager _userManager;
        private readonly IValidator<RegisterRequestModel> _registerRequestModelValidator;
        private readonly IValidator<LoginRequestModel> _loginRequestModelValidator;

        public AuthController(IUserManager userManager, IValidator<RegisterRequestModel> registerRequestModelValidator, IValidator<LoginRequestModel> loginRequestModelValidator)
        {
            _userManager = userManager;
            _registerRequestModelValidator = registerRequestModelValidator;
            _loginRequestModelValidator = loginRequestModelValidator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel loginRequestModel)
        {
            var result = _loginRequestModelValidator.Validate(loginRequestModel);

            if (!result.IsValid)
            {
                return BadRequest(new BaseResponseModel().ToErrorResponse(result.Errors[0].ErrorMessage));
            }

            var response = await _userManager.GetUser(loginRequestModel);

            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestModel registerRequestModel)
        {
            var result = _registerRequestModelValidator.Validate(registerRequestModel);

            if (!result.IsValid)
            {
                return BadRequest(new BaseResponseModel().ToErrorResponse(result.Errors[0].ErrorMessage));
            }

            var response = await _userManager.CreateUser(registerRequestModel);

            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}