using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleAuthApp.Services;
using SimpleAuthApp.ViewModels;

namespace SimpleAuthApp.Controllers
{
    [ApiController]
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userService;

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginViewModel user)
        {
            var jwt = _userService.Authenticate(user.Email, user.Password);

            if (jwt != null)
            {
                return Ok(jwt);
            }

            return Unauthorized();
        }

        [Authorize]
        [HttpGet("who-am-i")]
        public IActionResult WhoAmI()
        {
            return Content($"Hello {User.Identity.Name}");
        }
    }
}