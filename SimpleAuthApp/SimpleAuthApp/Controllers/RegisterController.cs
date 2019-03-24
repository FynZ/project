using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimpleAuthApp.Models;
using SimpleAuthApp.Services;
using SimpleAuthApp.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SimpleAuthApp.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IUserService _userService;

        public RegisterController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                _userService.CreateUser(new User
                {
                    Email = registerViewModel.Email,
                    Username = registerViewModel.Username,
                }, registerViewModel.Password);
            }

            return BadRequest();
        }
    }
}
