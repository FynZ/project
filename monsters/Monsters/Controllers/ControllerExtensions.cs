using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Accounts.Controllers
{
    public static class ControllerExtensions
    {
        public static int AuthenticatedUserId(this ControllerBase controller)
        {
            return Int32.Parse(controller.User.Claims.First(x => x.Type == "id").Value);
        }
    }
}
