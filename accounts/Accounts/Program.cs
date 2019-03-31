using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using Accounts.Configuration.Extensions;

namespace Accounts
{
    public class Program
    {
        public static IConfiguration Configuration { get; private set; }

        public static async Task<int> Main(string[] args)
        {
            try
            {
                Program.InitializeConfiguration();
                Program.InitializeLogging();

                await WebHost.CreateDefaultBuilder(args)
                    .UseStartup<Startup>()
                    .UseUrls("http://*:80")
                    .UseConfiguration(Configuration)
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

        private static void InitializeLogging()
        {
            SelfLog.Enable(Console.Error);

            var assembly = Assembly.GetEntryAssembly();

            var logCfg = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .Enrich.WithExceptionDetails()
                .Enrich.WithProperty("Application", assembly.GetName().Name)
                .Enrich.WithProperty("Version", FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion)
                .Enrich.WithProperty("Environment", Configuration["ASPNETCORE_ENVIRONMENT"])
                .WriteTo.Console();

            var elasticSearchUri = Configuration["ElasticSearchUri"];
            var indexPrefixTemplate = Configuration["ElasticSearchIndexPrefix"];

            if (!string.IsNullOrEmpty(elasticSearchUri) && !string.IsNullOrEmpty(indexPrefixTemplate))
            {
                Console.WriteLine("Elastic Logs will be stored at " + elasticSearchUri);
                logCfg.WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticSearchUri))
                {
                    AutoRegisterTemplate = true,
                    BatchPostingLimit = 50,
                    InlineFields = true,
                    MinimumLogEventLevel = LogEventLevel.Debug,
                    BufferFileSizeLimitBytes = 5242880,
                    IndexFormat = indexPrefixTemplate + "-{0:yyyy.MM}"
                });
            }

            Log.Logger = logCfg.CreateLogger();
        }

        private static void InitializeConfiguration()
        {
            var environmentVariable =
                Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                ?? "development";

            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{environmentVariable}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
