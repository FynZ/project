using System.Collections.Generic;
using System.IO.Compression;
using Accounts.Repositories;
using Accounts.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Monsters.Configuration.Extensions;
using Monsters.Configuration.Security;
using Swashbuckle.AspNetCore.Swagger;

namespace Monsters
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
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
                    o.UseGeneralRoutePrefix("monsters");
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Monster Microservice", Version = "v1" });
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

            // Dependency Injection registration
            services
                .AddScoped<IMonsterRepository, MonsterRepository>(x => new MonsterRepository(Configuration.GetConnectionString("Postgres")))
                .AddScoped<IMonsterService, MonsterService>();
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Authentication Microservice");
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
