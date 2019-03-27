using Microsoft.AspNetCore.Mvc;
using SimpleAuthApp.Services;
using SimpleAuthApp.ViewModels;

namespace SimpleAuthApp.Controllers
{
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userService;

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] LoginViewModel user)
        {
            var jwt = _userService.Authenticate(user.Email, user.Password);

            if (jwt != null)
            {
                return Ok(jwt);
            }

            return Unauthorized();
        }

        [Route("who-am-i")]
        public IActionResult WhoAmI()
        {
            return Content($"Hello {User.Identity.Name}");
        }
    }
}