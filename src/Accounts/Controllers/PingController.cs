using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.Controllers
{
    [ApiController]
    [EnableCors("Default")]
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