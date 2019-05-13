using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using WebApi.Shared.Configuration.Extensions;
using WebApi.Shared.Configuration.Program;

namespace Trading
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            try
            {
                var environment =
                    Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                    ?? "development";

                var configuration = ProgramConfiguration.InitializeConfiguration(environment);
                ProgramConfiguration.InitializeLogging(configuration);

                if (environment != "Development")
                {
                    var delay = 30 * 1000;
                    Console.WriteLine($"Waiting {delay / 1000} seconds for dependencies to be ready");
                    await Task.Delay(delay);
                }

                await WebHost.CreateDefaultBuilder(args)
                    .UseStartup<Startup>()
                    .UseUrls($"http://*:{(Int32.TryParse(configuration["Port"], out var port) ? port : 80)}")
                    .UseConfiguration(configuration)
                    .AddLogging()
                    .AddMetrics()
                    .Build()
                    .RunAsync();

                return 0;
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Host terminated unexpectedly!");

                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
