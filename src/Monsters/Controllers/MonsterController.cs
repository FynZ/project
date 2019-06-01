using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Monsters.DTO;
using Monsters.Services;
using WebApi.Shared.Extensions;

namespace Monsters.Controllers
{
    [ApiController]
    [EnableCors("Default")]
    public class MonsterController : ControllerBase
    {
        private readonly IMonsterService _monsterService;

        public MonsterController(IMonsterService monsterService)
        {
            _monsterService = monsterService;
        }

        [HttpGet("summary")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "User")]
        [ProducesResponseType(typeof(MonsterSummary), (int)HttpStatusCode.OK)]
        public IActionResult GetUserMonsterSummary()
        {
            var summary = _monsterService.GetSummary(this.AuthenticatedUserId());

            if (summary != null)
            {
                return Ok(summary);
            }

            return NotFound();
        }

        [HttpGet("")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "User")]
        public IActionResult GetUserMonsters()
        {
            var monsters = _monsterService.GetUserMonsters(this.AuthenticatedUserId());

            return Ok(monsters);
        }

        [HttpGet("increment/{monsterId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "User")]
        public IActionResult Increment(int monsterId)
        {
            _monsterService.IncrementMonster(monsterId, this.AuthenticatedUserId());

            return Ok();
        }

        [HttpGet("decrement/{monsterId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "User")]
        public IActionResult Decrement(int monsterId)
        {
            _monsterService.DecrementMonster(monsterId, this.AuthenticatedUserId());

            return Ok();
        }

        [HttpGet("propose/{monsterId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "User")]
        public IActionResult Propose(int monsterId)
        {
            _monsterService.ProposeMonster(monsterId, this.AuthenticatedUserId());

            return Ok();
        }

        [HttpGet("unpropose/{monsterId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "User")]
        public IActionResult Unpropose(int monsterId)
        {
            _monsterService.UnproposeMonster(monsterId, this.AuthenticatedUserId());

            return Ok();
        }

        [HttpGet("search/{monsterId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "User")]
        public IActionResult Search(int monsterId)
        {
            _monsterService.SearchMonster(monsterId, this.AuthenticatedUserId());

            return Ok();
        }

        [HttpGet("unsearch/{monsterId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "User")]
        public IActionResult Unsearch(int monsterId)
        {
            _monsterService.UnsearchMonster(monsterId, this.AuthenticatedUserId());

            return Ok();
        }
    }
}