using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace Trading.Configuration.Extensions
{
    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder AddLogging(this IWebHostBuilder app)
        {
            return app.UseSerilog();
        }

        public static IWebHostBuilder AddMetrics(this IWebHostBuilder app)
        {
            return app;
        }
    }
}
