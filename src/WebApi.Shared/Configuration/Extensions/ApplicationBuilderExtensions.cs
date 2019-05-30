using System.Reflection;
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

        public static IApplicationBuilder UseSwagger(this IApplicationBuilder builder, string assemblyName)
        {
            builder.UseSwagger(
                    c =>
                    {
                        c.PreSerializeFilters.Add((swagger, httpReq) => swagger.Host = httpReq.Host.Value);
                    });
            builder.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{assemblyName} Microservice");
            });

            return builder;
        }
    }
}
