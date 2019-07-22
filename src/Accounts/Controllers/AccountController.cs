using System.Collections.Generic;
using System.Net;
using Accounts.DTO;
using Accounts.Models;
using Accounts.Services;
using Accounts.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WebApi.Shared.Enumerations;
using WebApi.Shared.Extensions;

namespace Accounts.Controllers
{
    [ApiController]
    [EnableCors("Default")]
    public class AccountController : ControllerBase
    {
        private readonly IProfileService _profileService;
        private readonly IManagementService _managementService;

        public AccountController(IProfileService profileService, IManagementService managementService)
        {
            _profileService = profileService;
            _managementService = managementService;
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

        [HttpGet("users")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(Role.Admin))]
        [ProducesResponseType(typeof(IEnumerable<UserManagement>), (int)HttpStatusCode.OK)]
        public IActionResult GetUsersProfiles()
        {
            var users = _managementService.GetUsers();

            if (users != null)
            {
                return Ok(users);
            }

            return NotFound();
        }

        [HttpPost("user/ban/{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(Role.Admin))]
        public IActionResult BanUser(int userId)
        {
            _managementService.BanUser(userId);

            return Ok();
        }

        [HttpPost("user/unban/{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(Role.Admin))]
        public IActionResult UnbanUser(int userId)
        {
            _managementService.UnbanUser(userId);

            return Ok();
        }
    }
}
