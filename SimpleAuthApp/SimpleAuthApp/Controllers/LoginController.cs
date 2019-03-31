using System;
using System.Buffers.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using SimpleAuthApp.Configuration.Security;
using SimpleAuthApp.Configuration.Security.Models;
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

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginViewModel user)
        {
            var jwt = _userService.Authenticate(user.Email, user.Password);

            if (jwt != null)
            {
                return Ok(jwt);
            }

            return Unauthorized(new
            {
                status = HttpStatusCode.Unauthorized,
                message = "Email or Password is incorrect"
            });
        }

        [AllowAnonymous]
        [HttpGet("who-am-i")]
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