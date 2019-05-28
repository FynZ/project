using Accounts.Models.Enumerations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounts.Controllers
{
    [ApiController]
    [EnableCors("Default")]

    public class ServerController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet("servers")]
        public IActionResult GetServers()
        {
            return Ok(Enum.GetValues(typeof(Server)).Cast<Server>());
        }
    }
}
