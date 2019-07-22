using System.IdentityModel.Tokens.Jwt;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Accounts.DTO;
using Accounts.Services;
using Accounts.Services.Security.Models;
using Accounts.ViewModels;
using Microsoft.AspNetCore.Cors;

namespace Accounts.Controllers
{
    [ApiController]
    [EnableCors("Default")]
    public class LoginController : ControllerBase
    {
        private readonly IUserService _userService;

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(Jwt), (int)HttpStatusCode.OK)]
        public IActionResult Login([FromBody] LoginViewModel user)
        {
            var result = _userService.Authenticate(user.Email, user.Password);

            if (result.AuthenticationOutcome == AuthResult.Success)
            {
                return Ok(result.Jwt);
            }
            else if (result.AuthenticationOutcome == AuthResult.InvalidEmail || result.AuthenticationOutcome == AuthResult.InvalidPassword)
            {
                return Unauthorized(new
                {
                    status = HttpStatusCode.Unauthorized,
                    message = "Email or Password is incorrect"
                });
            }
            else if (result.AuthenticationOutcome == AuthResult.NotVerified)
            {
                return Unauthorized(new
                {
                    status = HttpStatusCode.Unauthorized,
                    message = "Account has not been activated"
                });
            }
            else if (result.AuthenticationOutcome == AuthResult.Banned)
            {
                return Unauthorized(new
                {
                    status = HttpStatusCode.Unauthorized,
                    message = "Account banned"
                });
            }

            return Unauthorized(new
            {
                status = HttpStatusCode.Unauthorized,
                message = "Unauthorized for unknown reason"
            });
        }

        [AllowAnonymous]
        [HttpGet("who-am-i")]
        [ProducesResponseType(typeof(WhoAmI), (int)HttpStatusCode.OK)]
        public IActionResult WhoAmI()
        {
            if (User.Identity.IsAuthenticated)
            {
                HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues value);

                var handler = new JwtSecurityTokenHandler();

                var jsonToken = handler.ReadToken(value.ToString().Replace("Bearer ", "")) as JwtSecurityToken;

                return Ok(new WhoAmI
                {
                    authenticated = true,
                    payload = jsonToken?.Payload
                });
            }

            return Ok(new WhoAmI
            {
                authenticated = false,
                payload = null
            });
        }
    }
}