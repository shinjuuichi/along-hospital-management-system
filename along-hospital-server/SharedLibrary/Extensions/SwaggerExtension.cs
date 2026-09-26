using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SharedLibrary.Commons;
using System.Reflection;

namespace SharedLibrary.Extensions
{
    public static class SwaggerExtension
    {
        public static IServiceCollection AddSwaggerServices(this IServiceCollection services, AppConfiguration configuration
            )
        {
            services.AddSwaggerGen(option =>
            {
                var title = configuration.AppInfo?.Name
                   ?? Assembly.GetEntryAssembly()?.GetName().Name
                   ?? "Hospital";

                var version = configuration.AppInfo?.Version
                              ?? Assembly.GetEntryAssembly()?.GetName().Version?.ToString()
                              ?? "v1";
                option.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = title,
                    Version = version,
                });

                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Valid Token is needed",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });

            return services;
        }

        public static WebApplication UseSwaggerServices(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            return app;
        }
    }
}