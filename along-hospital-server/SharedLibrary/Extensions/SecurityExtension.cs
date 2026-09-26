using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.Commons;
using SharedLibrary.Commons.Results;
using System.Text;

namespace SharedLibrary.Extensions
{
    public static class SecurityExtension
    {
        private const string CorsPolicy = "AlongCorsPolicy";

        public static IServiceCollection AddSecurityServices(this IServiceCollection services, AppConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration.JwtConfig.Issuer,
                    ValidAudience = configuration.JwtConfig.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.JwtConfig.SecretKey)),
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;

                        if (!string.IsNullOrWhiteSpace(accessToken) && path.StartsWithSegments("/hubs"))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    },

                    OnChallenge = async context =>
                    {
                        context.HandleResponse();

                        if (!context.Response.HasStarted)
                        {
                            var isTokenExpired = context.AuthenticateFailure is SecurityTokenExpiredException;

                            var errorMsg = isTokenExpired
                                ? "Your session has expired. Please log in again"
                                : "You must be logged in to perform this action";

                            var result = Result.FailErrors(
                                [errorMsg],
                                errorMsg,
                                StatusCodes.Status401Unauthorized);

                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/json";

                            if (result is ObjectResult objectResult)
                            {
                                await context.Response.WriteAsJsonAsync(objectResult.Value);
                            }
                        }
                    },

                    OnForbidden = async context =>
                    {
                        if (!context.Response.HasStarted)
                        {
                            var result = Result.FailErrors(
                                ["You do not have permission to perform this action"],
                                "Forbidden",
                                StatusCodes.Status403Forbidden);

                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                            context.Response.ContentType = "application/json";

                            if (result is ObjectResult objectResult)
                            {
                                await context.Response.WriteAsJsonAsync(objectResult.Value);
                            }
                        }
                    },

                    OnAuthenticationFailed = context =>
                    {
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicy, policy =>
                {
                    var frontendUrl = !string.IsNullOrWhiteSpace(configuration.UrlsConfig?.FrontendUrl)
                        ? configuration.UrlsConfig.FrontendUrl
                        : "http://localhost:3000";
                    var origins = new[]
                    {
                    frontendUrl,
                    "http://localhost:5173",
                    "http://localhost:3000",
                    "https://along-hospital.vercel.app",
                    "https://localhost:3000",
                    }.Distinct().ToArray();

                    policy.WithOrigins(origins)
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
                });
            });

            return services;
        }

        public static WebApplication UseSecurityServices(this WebApplication app)
        {
            app.UseRouting();

            app.UseCors(CorsPolicy);

            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}
