using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Debugging;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;

namespace WebApi.Shared.Configuration.Program
{
    public static class ProgramConfiguration
    {
        public static void InitializeLogging(IConfiguration configuration)
        {
            SelfLog.Enable(Console.Error);

            var assembly = Assembly.GetEntryAssembly() ?? throw new Exception("Unable to find entry assembly");

            var logCfg = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.WithExceptionDetails()
                .Enrich.WithProperty("Application", assembly.GetName().Name )
                .Enrich.WithProperty("Version", FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion)
                .Enrich.WithProperty("Environment", configuration["ASPNETCORE_ENVIRONMENT"])
                .WriteTo.Console();

            var enableElasticSearchLogging = Boolean.Parse(configuration["EnableElasticSearchLogging"]);

            if (enableElasticSearchLogging)
            {
                var elasticSearchUri = configuration["ElasticSearchUri"];
                var indexPrefixTemplate = configuration["ElasticSearchIndexPrefix"];

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

        public static IConfiguration InitializeConfiguration(string environment)
        {
            Console.WriteLine($"Configuration is {environment}");

            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
