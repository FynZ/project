using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = JwtHandler.InitializeJwtParameters();
                });

            services
                .AddMvc(o => { o.UseGeneralRoutePrefix("auth"); })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Config to Obj registration
            services
                .Configure<JwtSettings>(Configuration.GetSection("jwt"));

            // DI registration
            services
                .AddSingleton<IJwtHandler, JwtHandler>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IUserService, UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

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
    }
}
