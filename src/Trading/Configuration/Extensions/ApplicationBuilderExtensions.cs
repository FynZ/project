using Microsoft.AspNetCore.Builder;
using Trading.Configuration.Middlewares;

namespace Trading.Configuration.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SerilogMiddleware>();
        }
    }
}
