using Auth.Api.Attributes;
using Auth.Api.Validators;
using Auth.Business.Interfaces;
using Auth.Common.Helper;
using Auth.Common.Models.Request;
using Auth.Common.Models.Response;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers
{
    [ApiController]
    [JwtIgnore]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserManager _userManager;
        private readonly GenericValidator _genericValidator;

        public AuthController(IUserManager userManager, GenericValidator genericValidator)
        {
            _userManager = userManager;
            _genericValidator = genericValidator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel loginRequestModel)
        {
            _genericValidator.Validate<LoginRequestModelValidator, LoginRequestModel>(loginRequestModel);

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
            _genericValidator.Validate<RegisterRequestModelValidator, RegisterRequestModel>(registerRequestModel);

            var response = await _userManager.CreateUser(registerRequestModel);

            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}