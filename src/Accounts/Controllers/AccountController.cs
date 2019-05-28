using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accounts.Services;
using Accounts.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WebApi.Shared.Extensions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Accounts.Controllers
{
    [ApiController]
    [EnableCors("Default")]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            var userInfo = _userService.GetUserInformations(this.AuthenticatedUserId());

            if (userInfo != null)
            {
                return Ok(userInfo);
            }

            return NotFound();
        }

        [Authorize]
        [HttpGet("profile/{userId}")]
        public IActionResult GetUserProfile(int userId)
        {
            var userInfo = _userService.GetUserProfile(userId);

            if (userInfo != null)
            {
                return Ok(userInfo);
            }

            return NotFound();
        }

        [Authorize]
        [HttpGet("profile/update")]
        public IActionResult UpdateProfile([FromBody] UpdateProfileViewModel updateProfileViewModel)
        {
            return null;
        }
    }
}
