using Microsoft.AspNetCore.Builder;
using WebApi.Shared.Configuration.Middlewares;

namespace WebApi.Shared.Configuration.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SerilogMiddleware>();
        }
    }
}
