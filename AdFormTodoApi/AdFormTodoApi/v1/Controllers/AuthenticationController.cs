using AdFormTodoApi.Core.Models;
using AdFormTodoApi.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdFormTodoApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;
        public AuthenticationController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Method to authenticate User
        /// </summary>
        /// <param name="User"></param>
        /// <returns>string</returns>
        [HttpPost("Authenticate")]
        [AllowAnonymous]
        public IActionResult Post(AuthenticateRequest request)
        {
            var response = _userService.Authenticate(request);

            if (response == null)
            {
                return BadRequest(new { message = "Username or Password is incorrect" });

            }
            else
            {
                return Ok(response);
            }
        }


    }
}
