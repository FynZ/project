using Microsoft.AspNetCore.Builder;
using Monsters.Configuration.Middlewares;

namespace Monsters.Configuration.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SerilogMiddleware>();
        }
    }
}
