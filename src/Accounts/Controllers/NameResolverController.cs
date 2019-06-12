using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Accounts.DTO;
using Accounts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WebApi.Shared.Extensions;

namespace Accounts.Controllers
{
    [ApiController]
    [EnableCors("Default")]
    [Route("resolve")]
    public class NameResolverController : ControllerBase
    {
        private readonly IUserResolverService _userResolverService;

        public NameResolverController(IUserResolverService userResolverService)
        {
            _userResolverService = userResolverService;
        }

        [Authorize]
        [HttpGet("by-id/{userId}")]
        [ProducesResponseType(typeof(SimpleUser), (int)HttpStatusCode.OK)]
        public IActionResult GetProfile(int userId)
        {
            var userInfo = _userResolverService.GetUserById(userId);

            if (userInfo != null)
            {
                return Ok(userInfo);
            }

            return NotFound();
        }
    }
}