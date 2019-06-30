using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Monsters.Services;
using WebApi.Shared.Enumerations;
using WebApi.Shared.Extensions;

namespace Monsters.Controllers
{
    [ApiController]
    [EnableCors("Default")]
    public class ManagementController : ControllerBase
    {
        private readonly IManagementService _managementService;

        public ManagementController(IManagementService managementService)
        {
            _managementService = managementService;
        }

        [HttpPost("increment/{monsterId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(Role.User))]
        public IActionResult Increment(int monsterId)
        {
            _managementService.IncrementMonster(monsterId, this.AuthenticatedUserId());

            return Ok();
        }

        [HttpPost("decrement/{monsterId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(Role.User))]
        public IActionResult Decrement(int monsterId)
        {
            _managementService.DecrementMonster(monsterId, this.AuthenticatedUserId());

            return Ok();
        }

        [HttpPost("propose/{monsterId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(Role.User))]
        public IActionResult Propose(int monsterId)
        {
            _managementService.ProposeMonster(monsterId, this.AuthenticatedUserId());

            return Ok();
        }

        [HttpPost("unpropose/{monsterId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(Role.User))]
        public IActionResult Unpropose(int monsterId)
        {
            _managementService.UnproposeMonster(monsterId, this.AuthenticatedUserId());

            return Ok();
        }

        [HttpPost("search/{monsterId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(Role.User))]
        public IActionResult Search(int monsterId)
        {
            _managementService.SearchMonster(monsterId, this.AuthenticatedUserId());

            return Ok();
        }

        [HttpPost("unsearch/{monsterId}")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(Role.User))]
        public IActionResult Unsearch(int monsterId)
        {
            _managementService.UnsearchMonster(monsterId, this.AuthenticatedUserId());

            return Ok();
        }
    }
}