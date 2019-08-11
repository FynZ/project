using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Trading.Services;
using WebApi.Shared.Enumerations;
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
        [Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(Role.User))]
        public IActionResult GetUserMonsters()
        {
            var monsters = _tradingService.GetTradableUsers(this.AuthenticatedUserId());

            return Ok(monsters);
        }

        [HttpGet("{targetId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(Role.User))]
        public IActionResult GetTradingDetails(int targetId)
        {
            var details = _tradingService.GetDetailTrade(this.AuthenticatedUserId(), targetId);

            return Ok(details);
        }

        [HttpGet("search/{targetId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(Role.User))]
        public IActionResult GetSearchedMonstersWithMatchs(int targetId)
        {
            var monsters = _tradingService.GetSearchedMonstersWithMatchs(this.AuthenticatedUserId(), targetId);

            return Ok(monsters);
        }

        [HttpGet("propose/{targetId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(Role.User))]
        public IActionResult GetProposedMonstersWithMatchs(int targetId)
        {
            var monsters = _tradingService.GetProposedMonstersWithMatchs(this.AuthenticatedUserId(), targetId);

            return Ok(monsters);
        }
    }
}