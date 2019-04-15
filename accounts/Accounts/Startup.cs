using System.IO.Compression;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Accounts.Configuration.Extensions;
using Accounts.Configuration.Security;
using Accounts.Repositories;
using Accounts.Services;
using Accounts.Settings;
using Swashbuckle.AspNetCore.Swagger;

namespace Accounts
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
                    o.UseGeneralRoutePrefix("auth");
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Authentication Microservice", Version = "v1" });
                c.DescribeAllEnumsAsStrings();
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
                .Configure<JwtSettings>(Configuration.GetSection("jwtCreation"));

            // Dependency Injection registration
            services
                .AddSingleton<IJwtHandler, JwtHandler>()
                .AddScoped<IUserRepository, UserRepository>(x => new UserRepository(Configuration.GetConnectionString("Postgres")))
                .AddScoped<IUserService, UserService>();
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Authentication Microservice");
            });
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
