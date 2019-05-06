using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Trading.Services;

namespace Trading.Controllers
{
    [ApiController]
    [EnableCors("Default")]
    public class TradingController : ControllerBase
    {
        private readonly ITradingService _tradingService;

        public TradingController(ITradingService tradingService)
        {
            _tradingService = tradingService;
        }

        [HttpGet("")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "User")]
        public IActionResult GetUserMonsters()
        {
            var monsters = _tradingService.GetTradableUsers(this.AuthenticatedUserId());

            return Ok(monsters);
        }
    }
}