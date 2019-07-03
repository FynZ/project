using System;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Steeltoe.Discovery.Client;
using Trading.Repositories;
using Trading.Services;
using Trading.Settings;
using WebApi.Shared.Configuration;
using WebApi.Shared.Configuration.Extensions;
using WebApi.Shared.Configuration.Security;

namespace Trading
{
    public class Startup
    {
        public string AssemblyName { get; }
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            AssemblyName = Assembly.GetEntryAssembly()?.GetName().Name ?? throw new Exception("Assembly name not found");
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Cors
            services.AddDefaultCorsConfiguration();
            // Mvc
            services.AddDefaultMvcConfiguration(Configuration["RoutePrefix"]);
            // Swagger
            services.AddDefaultSwaggerConfiguration(AssemblyName);
            // Compression
            services.AddDefaultCompression();

            // Service Discovery
            services.AddDiscoveryClient(Configuration);

            // Authentication
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = PublicKeyManager.InitializeJwtParameters();
                });

            // Config to Object registration
            services
                .Configure<RabbitMQSettings>(Configuration.GetSection("rabbitMQ"));

            // Dependency Injection registration
            services
                .AddSingleton<ITradingRepository, TradingRepository>(x => new TradingRepository(Configuration.GetConnectionString("Postgres")))
                .AddSingleton<ITradingService, TradingService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app
            .UseForwardedHeaders(ApplicationHelper.GetDefaultForwardedHeadersOptions())
            .UseResponseCompression()
            .UseLoggingMiddleware()
            .UseAuthentication()
            .UseMvc()
            .UseSwagger(AssemblyName)
            .UseDiscoveryClient();
        }
    }
}
