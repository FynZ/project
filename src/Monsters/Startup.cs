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
using Microsoft.Extensions.Hosting;
using Monsters.Repositories;
using Monsters.Services;
using Monsters.Services.HostedServices;
using Monsters.Services.HostedServices.Communication;
using Monsters.Settings;
using Swashbuckle.AspNetCore.Swagger;
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
            services.AddCors(o => o.AddPolicy("Default", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            // Authentication
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
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
            .UseForwardedHeaders(GetForwardedHeadersOptions())
            .UseResponseCompression()
            .UseLoggingMiddleware()
            .UseAuthentication()
            .UseMvc()
            .UseSwagger(
            c =>
            {
                c.PreSerializeFilters.Add((swagger, httpReq) => swagger.Host = httpReq.Host.Value);
            })
            .UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{AssemblyName} Microservice");
            });
        }

        public static ForwardedHeadersOptions GetForwardedHeadersOptions()
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
