using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace WebApi.Shared.Extensions
{
    public static class ControllerExtensions
    {
        public static int AuthenticatedUserId(this ControllerBase controller)
        {
            return Int32.Parse(controller.User.Claims.First(x => x.Type == "id").Value);
        }
    }
}
