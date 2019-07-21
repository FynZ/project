using System;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Steeltoe.Discovery.Client;
using WebApi.Shared.Configuration;
using WebApi.Shared.Configuration.Extensions;
using WebApi.Shared.Configuration.Security;
using Accounts.Repositories;
using Accounts.Services;
using Accounts.Services.HostedServices;
using Accounts.Services.HostedServices.Communication;
using Accounts.Services.Security;
using Accounts.Settings;

namespace Accounts
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
                .Configure<JwtSettings>(Configuration.GetSection("jwtCreation"))
                .Configure<RabbitMQSettings>(Configuration.GetSection("rabbitMQ"));

            // Dependency Injection registration
            services
                .AddSingleton<IJwtHandler, JwtHandler>()
                .AddSingleton<IUserRepository, UserRepository>(x => new UserRepository(Configuration.GetConnectionString("Postgres")))
                .AddSingleton<IUserService, UserService>()
                .AddSingleton<IProfileService, ProfileService>()
                .AddSingleton<IManagementService, ManagementService>()
                .AddTransient<IHostedServiceAccessor<IMonsterServiceCommunication>, HostServiceAccessor<IMonsterServiceCommunication>>()
                .AddSingleton<IHostedService, MonsterServiceCommunication>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app
            .UseForwardedHeaders(ApplicationHelper.GetDefaultForwardedHeadersOptions())
            .UseResponseCompression()
            .UseAuthentication()
            .UseLoggingMiddleware()
            .UseMvc()
            .UseSwagger(AssemblyName)
            .UseDiscoveryClient();
        }
    }
}
