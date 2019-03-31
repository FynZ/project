using System.IO.Compression;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleAuthApp.Configuration;
using SimpleAuthApp.Configuration.Extensions;
using SimpleAuthApp.Configuration.Security;
using SimpleAuthApp.Extensions;
using SimpleAuthApp.Repositories;
using SimpleAuthApp.Services;
using SimpleAuthApp.Settings;

namespace SimpleAuthApp
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
                    options.TokenValidationParameters = JwtHandler.InitializeJwtParameters();
                });

            // Mvc
            services
                .AddMvc(o =>
                {
                    o.UseGeneralRoutePrefix("auth");
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Swagger
            //services.AddSwaggerGen();

            // Compression
            //services.Configure<GzipCompressionProviderOptions>
            //    (options => options.Level = CompressionLevel.Optimal);
            //services.AddResponseCompression(options =>
            //{
            //    options.Providers.Add<GzipCompressionProvider>();
            //});

            // Config to Object registration
            services
                .Configure<JwtSettings>(Configuration.GetSection("jwt"));

            // Dependency Injection registration
            services
                .AddSingleton<IJwtHandler, JwtHandler>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IUserService, UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //app
            //.UseForwardedHeaders(GetForwardedHeadersOptions())
            //.UseResponseCompression()
            //.UseLoggingMiddleware()
            //.UseAuthentication()
            //.UseMvc();
            //.UseSwagger()
            //.UseSwaggerUI();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseMvc();
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
