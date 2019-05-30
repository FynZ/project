using System.Net;
using Accounts.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Accounts.Services;
using Accounts.ViewModels;
using Microsoft.AspNetCore.Cors;

namespace Accounts.Controllers
{
    [ApiController]
    [EnableCors("Default")]
    public class RegisterController : ControllerBase
    {
        private readonly IUserService _userService;

        public RegisterController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [ProducesResponseType(typeof(RegisterResult), (int)HttpStatusCode.OK)]
        public IActionResult Register([FromBody] RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var result = _userService.CreateUser(new UserCreation
                {
                    Username = registerViewModel.Username,
                    Email = registerViewModel.Email,
                    Password = registerViewModel.Password,
                    Server = registerViewModel.Server,
                    InGameName = registerViewModel.InGameName,
                    Subscribed = registerViewModel.Subscribe,
                });

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
