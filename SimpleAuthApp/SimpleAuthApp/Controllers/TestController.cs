using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SimpleAuthApp.Controllers
{
    /// <summary>
    /// Testing class for development purpose only
    /// </summary>
    [ApiController]
    public class TestController : ControllerBase
    {
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
            if (User.Identity.IsAuthenticated)
            {
                return Ok(User.Claims);
            }

            return Unauthorized();

            /*
            var sb = new StringBuilder();
            foreach (var claim in User.Claims)
            {
                sb.Append(claim.Type);
                sb.Append(" | ");
                sb.Append(claim.Value);
                sb.Append(Environment.NewLine);
            }

            return Content(sb.ToString());
            */
        }

        [Route("claim")]
        public IActionResult TestClaim()
        {
            return Content("working");
        }
    }
}