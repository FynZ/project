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

        [HttpGet("increment/{monsterId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "User")]
        public IActionResult Increment(int monsterId)
        {
            return Content($"Request made by user {this.AuthenticatedUserId()}");
        }

        [HttpGet("decrement/{monsterId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "User")]
        public IActionResult Decrement(int monsterId)
        {
            return Content($"Request made by user {this.AuthenticatedUserId()}");
        }

        [HttpGet("propose/{monsterId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "User")]
        public IActionResult Propose(int monsterId)
        {
            return Content($"Request made by user {this.AuthenticatedUserId()}");
        }

        [HttpGet("search/{monsterId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "User")]
        public IActionResult Search(int monsterId)
        {
            return Content($"Request made by user {this.AuthenticatedUserId()}");
        }

        [HttpGet("init/{userId}")]
        [AllowAnonymous]
        public IActionResult Init(int userId)
        {
            return null;

        }
    }
}