using System;
using System.Net;
using Accounts.DTO;
using Accounts.Models;
using Accounts.Services;
using Accounts.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WebApi.Shared.Extensions;

namespace Accounts.Controllers
{
    [ApiController]
    [EnableCors("Default")]
    public class AccountController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public AccountController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [Authorize]
        [HttpGet("profile")]
        [ProducesResponseType(typeof(UserInformation), (int)HttpStatusCode.OK)]
        public IActionResult GetProfile()
        {
            var userInfo = _profileService.GetUserInformation(this.AuthenticatedUserId());

            if (userInfo != null)
            {
                return Ok(userInfo);
            }

            return NotFound();
        }

        [Authorize]
        [HttpPost("profile/update")]
        public IActionResult UpdateProfile([FromBody] UpdateProfileViewModel updateProfileViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Id = this.AuthenticatedUserId(),
                    Email = updateProfileViewModel.Email,
                    Server = updateProfileViewModel.Server,
                    InGameName = updateProfileViewModel.InGameName,
                    Subscribed = updateProfileViewModel.Subscribed
                };

                if (!_profileService.UpdateUserProfile(user))
                {
                    return Conflict();
                }

                return Ok();
            }

            return BadRequest();
        }

        [Authorize]
        [HttpPost("profile/complete-update")]
        public IActionResult UpdateProfile([FromBody] UpdateProfileWithPasswordViewModel updateProfileViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Id = this.AuthenticatedUserId(),
                    Email = updateProfileViewModel.Email,
                    Server = updateProfileViewModel.Server,
                    InGameName = updateProfileViewModel.InGameName,
                    Subscribed = updateProfileViewModel.Subscribed
                };

                if (!_profileService.UpdateUserProfile(user, updateProfileViewModel.Password))
                {
                    return Conflict();
                }

                return Ok();
            }

            return BadRequest();
        }

        [Authorize]
        [HttpGet("users/{userId}")]
        [ProducesResponseType(typeof(UserProfile), (int)HttpStatusCode.OK)]
        public IActionResult GetUserProfile(int userId)
        {
            var userInfo = _profileService.GetUserProfile(userId);

            if (userInfo != null)
            {
                return Ok(userInfo);
            }

            return NotFound();
        }
    }
}
