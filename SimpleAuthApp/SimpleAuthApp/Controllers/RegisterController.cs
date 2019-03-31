using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleAuthApp.Models;
using SimpleAuthApp.Services;
using SimpleAuthApp.ViewModels;

namespace SimpleAuthApp.Controllers
{
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IUserService _userService;

        public RegisterController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = _userService.CreateUser(new User
                {
                    Username = registerViewModel.Username,
                    Email = registerViewModel.Email,
                }, registerViewModel.Password);

                if (result.WasCreated)
                {
                    return Ok(result);
                }

                return Conflict(result);
            }

            return BadRequest();
        }
    }
}
