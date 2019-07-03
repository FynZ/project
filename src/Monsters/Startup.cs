using System;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
using WebApi.Shared.Configuration.Security;

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
                .AddSingleton<IMonsterRepository, MonsterRepository>(x => new MonsterRepository(Configuration.GetConnectionString("Postgres")))
                .AddSingleton<IManagementService, ManagementService>()
                .AddSingleton<IMonsterService, MonsterService>()
                .AddSingleton<IMonsterIniter, ManagementService>()
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
