using System.Collections.Generic;
using System.IO.Compression;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace WebApi.Shared.Configuration.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDefaultCorsConfiguration(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddCors(o => o.AddPolicy("Default", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            return serviceCollection;
        }

        public static IServiceCollection AddDefaultMvcConfiguration(this IServiceCollection serviceCollection, string routePrefix)
        {
            serviceCollection
                .AddMvc(o =>
                {
                    //o.UseGeneralRoutePrefix(routePrefix);
                })
                .AddJsonOptions(o =>
                {
                    o.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            return serviceCollection;
        }

        public static IServiceCollection AddDefaultCompression(this IServiceCollection serviceCollection)
        {
            serviceCollection.Configure<GzipCompressionProviderOptions>
                (options => options.Level = CompressionLevel.Optimal);
            serviceCollection.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
            });

            return serviceCollection;
        }

        public static IServiceCollection AddDefaultSwaggerConfiguration(this IServiceCollection serviceCollection, string applicationName)
        {
            serviceCollection.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = $"{applicationName} Microservice", Version = "v1" });
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

            return serviceCollection;
        }
    }
}
