using Auth.Business.Interfaces;
using Auth.Common.Models.Request;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserManager _userManager;

        public UsersController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        [HttpPut("password")]
        public async Task<IActionResult> UpdatePassword([FromHeader(Name = "Authorization")] string token, [FromBody] UpdateRequestModel updateRequestModel)
        {
            var response = await _userManager.UpdatePassword(updateRequestModel, token);

            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}