using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using SimpleAuthApp.Configuration;

namespace SimpleAuthApp.Extensions
{
    public static class MvcOptionsExtensions
    {
        public static void UseGeneralRoutePrefix(this MvcOptions opts, string prefix)
        {
            opts.UseGeneralRoutePrefix(new RouteAttribute(prefix));
        }

        public static void UseGeneralRoutePrefix(this MvcOptions opts, IRouteTemplateProvider routeAttribute)
        {
            opts.Conventions.Add(new RoutePrefixConvention(routeAttribute));
        }
    }
}
