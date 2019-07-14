using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Prometheus;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace WebApi.Shared.Configuration.Extensions
{
    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder AddLogging(this IWebHostBuilder app)
        {
            return app.UseSerilog();
        }

        public static IWebHostBuilder AddMetrics(this IWebHostBuilder builder)
        {
            builder.UseHealthEndpoints()
                   .UseMetricsWebTracking()
                   .UseMetrics(options =>
                   {
                       options.EndpointOptions = endpointsOptions =>
                       {
                           endpointsOptions.MetricsEndpointOutputFormatter = new MetricsPrometheusProtobufOutputFormatter();
                           endpointsOptions.MetricsTextEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter();
                       };
                   });

            return builder;
        }
    }
}
