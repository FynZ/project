using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Monsters.DTO;
using Monsters.Models;
using Monsters.Services;
using WebApi.Shared.Enumerations;
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

        [HttpGet("")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(Role.User))]
        [ProducesResponseType(typeof(IEnumerable<UserMonster>), (int)HttpStatusCode.OK)]
        public IActionResult GetMonsters()
        {
            var monsters = _monsterService.GetUserMonsters(this.AuthenticatedUserId());

            return Ok(monsters);
        }

        [HttpGet("summary")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(Role.User))]
        [ProducesResponseType(typeof(MonsterSummary), (int)HttpStatusCode.OK)]
        public IActionResult GetSummary()
        {
            var summary = _monsterService.GetSummary(this.AuthenticatedUserId());

            if (summary != null)
            {
                return Ok(summary);
            }

            return NotFound();
        }

        [HttpGet("search")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(Role.User))]
        [ProducesResponseType(typeof(MonsterSummary), (int)HttpStatusCode.OK)]
        public IActionResult GetSearchedMonsters()
        {
            var monsters = _monsterService.GetSearchedMonsters(this.AuthenticatedUserId());

            return Ok(monsters);
        }

        [HttpGet("propose")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(Role.User))]
        [ProducesResponseType(typeof(MonsterSummary), (int)HttpStatusCode.OK)]
        public IActionResult GetProposedMonsters()
        {
            var monsters = _monsterService.GetProposedMonsters(this.AuthenticatedUserId());

            return Ok(monsters);
        }
    }
}