using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;
using Microsoft.AspNetCore.Http;

namespace ApiGateway
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiGatewayServices(
         this IServiceCollection services,
         IHostEnvironment env)
        {
            var ocelotConfig = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile($"ocelot.{env.EnvironmentName}.json", optional: false, reloadOnChange: true)
                .Build();

            services
                .AddOcelot(ocelotConfig)
                .AddCacheManager(x => x.WithDictionaryHandle())
                .AddPolly();

            return services;
        }

        public static WebApplication UseApiGatewayServices(this WebApplication app)
        {
            app.Use(async (context, next) =>
            {
                context.Response.OnStarting(() =>
                {
                    if (context.Response.StatusCode == StatusCodes.Status429TooManyRequests
                        && string.IsNullOrWhiteSpace(context.Response.ContentType))
                    {
                        context.Response.ContentType = "application/json; charset=utf-8";
                    }

                    return Task.CompletedTask;
                });

                await next();
            });

            app.UseOcelot().Wait();

            return app;
        }
    }
}
