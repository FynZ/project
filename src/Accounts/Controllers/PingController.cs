using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.Controllers
{
    [ApiController]
    public class PingController : Controller
    {
        [HttpGet("ping")]
        [AllowAnonymous]
        public ActionResult Ping()
        {
            return Content($"Pong");
        }
    }
}