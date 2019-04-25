using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Monsters.Controllers
{
    public static class ControllerExtensions
    {
        public static int AuthenticatedUserId(this ControllerBase controller)
        {
            return Int32.Parse(controller.User.Claims.First(x => x.Type == "id").Value);
        }
    }
}
