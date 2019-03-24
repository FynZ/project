using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimpleAuthApp.Services;
using SimpleAuthApp.ViewModels;

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
            return Json(_userService.GetAll());
        }
    }
}