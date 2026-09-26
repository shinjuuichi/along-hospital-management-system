using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Commons;
using SharedLibrary.Services.Implements;
using SharedLibrary.Services.Interfaces;

namespace SharedLibrary.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddBaseServices(this IServiceCollection services)
        {
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            return services;
        }

        public static IServiceCollection AddUploadService(this IServiceCollection services, AppConfiguration configuration)
        {
            services.AddHttpClient("UploadService", client =>
            {
                client.BaseAddress = new Uri(configuration.ApiUrlsConfig.UploadUrl);
                client.Timeout = TimeSpan.FromMinutes(5);
            });
            services.AddScoped<IUploadFileService, UploadFileService>();
            return services;
        }
    }
}
