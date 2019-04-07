using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Accounts.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Monsters.Controllers
{
    [ApiController]
    public class MonsterController : ControllerBase
    {
        [HttpGet("increment/{monsterId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "User")]
        public IActionResult Increment(int monsterId)
        {
            return Content($"Request made by user {this.AuthenticatedUserId()}");
        }
    }
}