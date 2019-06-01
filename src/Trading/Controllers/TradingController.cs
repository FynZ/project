using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Trading.Services;
using WebApi.Shared.Extensions;

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

        [HttpGet("{targetId}")]
        public IActionResult GetTradingDetails(int targetId)
        {
            var details = _tradingService.GetTradingDetails(this.AuthenticatedUserId(), targetId);

            return Ok(details);
        }
    }
}