using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Monsters.Repositories;
using Monsters.Services;
using Monsters.Services.HostedServices;
using Monsters.Services.HostedServices.Communication;
using Monsters.Settings;
using Steeltoe.Discovery.Client;
using WebApi.Shared.Configuration;
using WebApi.Shared.Configuration.Extensions;

namespace Monsters
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
            services.AddDefaultMvcConfiguration(AssemblyName);
            // Swagger
            services.AddDefaultSwaggerConfiguration(AssemblyName);
            // Compression
            services.AddDefaultCompression();

            // Service Discovery
            services.AddDiscoveryClient(Configuration);

            // Config to Object registration
            services
                .Configure<RabbitMQSettings>(Configuration.GetSection("rabbitMQ"));

            // Dependency Injection registration
            services
                .AddSingleton<IMonsterRepository, MonsterRepository>(x => new MonsterRepository(Configuration.GetConnectionString("Postgres")))
                .AddSingleton<IMonsterService, MonsterService>()
                .AddSingleton<IMonsterIniter, MonsterService>()
                .AddTransient<IHostedServiceAccessor<IAccountServiceCommunication>, HostServiceAccessor<IAccountServiceCommunication>>()
                .AddSingleton<IHostedService, AccountServiceCommunication>();
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
