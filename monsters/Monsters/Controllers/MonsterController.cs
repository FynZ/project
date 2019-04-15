using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accounts.Controllers;
using Accounts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Monsters.Controllers
{
    [ApiController]
    public class MonsterController : ControllerBase
    {
        private readonly IMonsterService _monsterService;

        public MonsterController(IMonsterService monsterService)
        {
            _monsterService = monsterService;
        }

        [HttpGet("init/{userId}")]
        [AllowAnonymous]
        public IActionResult Init(int userId)
        {
            _monsterService.InitUser(this.AuthenticatedUserId());

            return Ok();
        }

        [HttpGet("init/{userId}")]
        [AllowAnonymous]
        public IActionResult GetUserMonsters(int userId)
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

        [HttpGet("search/{monsterId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "User")]
        public IActionResult Search(int monsterId)
        {
            _monsterService.SearchMonster(monsterId, this.AuthenticatedUserId());

            return Ok();
        }
    }
}