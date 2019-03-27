using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleAuthApp.Models;
using SimpleAuthApp.Services;
using SimpleAuthApp.ViewModels;
using System.Security.Claims;

namespace SimpleAuthApp.Controllers
{
    [ApiController]
    public class LoginController : Controller
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
            return Json(_userService.Authenticate(user.Email, user.Password));
        }

        [Route("who-am-i")]
        public IActionResult WhoAmI()
        {
            return Content($"Hello {User.Identity.Name}");
            //return Json(_userService.GetAll());
        }

        [Route("user")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "User")]
        public IActionResult TestUser()
        {
            return Content($"Hello {User.Identity.Name}");
        }

        [Route("prout")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Prout")]
        public IActionResult TestProut()
        {
            return Content($"Hello {User.Identity.Name}");
        }

        [Route("claims")]
        public IActionResult TestClaims()
        {
            var sb = new StringBuilder();
            foreach (var claim in User.Claims)
            {
                sb.Append(claim.Type);
                sb.Append(" | ");
                sb.Append(claim.Value);
                sb.Append(Environment.NewLine);
            }

            return Content(sb.ToString());
        }

        [Route("claim")]
        //[Authorize(Policy = "User")]
        public IActionResult TestClaim()
        {
            return Content("working");
        }
    }
}