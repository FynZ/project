using Elasticsearch.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WebApi.Shared.Enumerations;

namespace Accounts.Controllers
{
    /// <summary>
    /// Testing class for development purpose only
    /// </summary>
    [ApiController]
    [EnableCors("Default")]
    public class TestController : ControllerBase
    {
        [HttpGet("user")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(Role.User))]
        public IActionResult TestUser()
        {
            return Content($"Hello {User.Identity.Name}");
        }

        [HttpGet("prout")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "Prout")]
        public IActionResult TestProut()
        {
            return Content($"Hello {User.Identity.Name}");
        }

        [HttpGet("claims")]
        public IActionResult TestClaims()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Ok(User.Claims);
            }

            return Unauthorized();
        }

        [HttpGet("claim")]
        public IActionResult TestClaim()
        {
            return Content("working");
        }
    }
}