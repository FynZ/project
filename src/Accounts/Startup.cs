using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Accounts.Configuration.Security;
using Accounts.Repositories;
using Accounts.Services;
using Accounts.Services.HostedServices;
using Accounts.Services.HostedServices.Communication;
using Accounts.Settings;
using Microsoft.Extensions.Hosting;
using Steeltoe.Discovery.Client;
using Swashbuckle.AspNetCore.Swagger;
using WebApi.Shared.Configuration.Extensions;
using WebApi.Shared.Configuration.Security;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

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
            services.AddDiscoveryClient(Configuration);

            services.AddCors(o => o.AddPolicy("Default", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            // Authentication
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = PublicKeyManager.InitializeJwtParameters(Configuration.GetSection("jwtValidation")["rsaPublicKeyXml"]);
                });

            // Mvc
            services
                .AddMvc(o =>
                {
                    o.UseGeneralRoutePrefix(Configuration?.GetValue("RoutePrefix", AssemblyName));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = $"{AssemblyName} Microservice", Version = "v1" });
                c.DescribeAllEnumsAsStrings();

                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { } }
                };

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "Bearer: {Your Token}",
                    Name = "Authorization",
                    In = "Header",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(security);
            });

            // Compression
            services.Configure<GzipCompressionProviderOptions>
                (options => options.Level = CompressionLevel.Optimal);
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
            });

            // Config to Object registration
            services
                .Configure<JwtSettings>(Configuration.GetSection("jwtCreation"))
                .Configure<RabbitMQSettings>(Configuration.GetSection("rabbitMQ"));

            // Dependency Injection registration
            services
                .AddSingleton<IJwtHandler, JwtHandler>()
                .AddSingleton<IUserRepository, UserRepository>(x => new UserRepository(Configuration.GetConnectionString("Postgres")))
                .AddSingleton<IMonsterIniter, MonsterIniter>()
                .AddSingleton<IUserService, UserService>()
                .AddTransient<IHostedServiceAccessor<IMonsterServiceCommunication>, HostServiceAccessor<IMonsterServiceCommunication>>()
                .AddSingleton<IHostedService, MonsterServiceCommunication>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app
            .UseForwardedHeaders(GetForwardedHeadersOptions())
            .UseResponseCompression()
            .UseAuthentication()
            .UseLoggingMiddleware()
            .UseMvc()
            .UseSwagger(
            c =>
            {
                c.PreSerializeFilters.Add((swagger, httpReq) => swagger.Host = httpReq.Host.Value);
            })
            .UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{AssemblyName} Microservice");
            })
            .UseDiscoveryClient();
        }

        private static ForwardedHeadersOptions GetForwardedHeadersOptions()
        {
            var forwardedOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            };
            forwardedOptions.KnownNetworks.Clear();
            forwardedOptions.KnownProxies.Clear();

            return forwardedOptions;
        }
    }
}
