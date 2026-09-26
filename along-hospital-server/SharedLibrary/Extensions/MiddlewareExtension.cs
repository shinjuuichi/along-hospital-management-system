using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Middlewares;

namespace SharedLibrary.Extensions
{
    public static class MiddlewareExtension
    {
        public static IServiceCollection AddMiddlewares(this IServiceCollection services)
        {
            services.AddScoped<ExceptionHandlingMiddleware>();

            return services;
        }

        public static WebApplication UseMiddlewares(this WebApplication app)
        {
            app.UseMiddleware<CorrelationIdMiddleware>();
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMiddleware<ProfileCompletionMiddleware>();
            app.UseMiddleware<ApiKeyMiddleware>();

            return app;
        }
    }
}