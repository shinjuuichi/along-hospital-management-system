using Microsoft.Extensions.DependencyInjection;
using QueueSvc.BLL.Implements;
using QueueSvc.BLL.Interfaces;
using QueueSvc.DAL.Data;
using SharedLibrary.Commons;
using SharedLibrary.Extensions;

namespace QueueSvc.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, AppConfiguration configuration)
        {
            services.AddApplicationDbContext<QueueDbContext>(configuration);
            services.AddAutoMapper(typeof(QueueMappingProfile).Assembly);
            services.AddRedisDatabase(configuration);

            services.AddScoped<IQueueQueryService, QueueQueryService>();
            services.AddScoped<IQueueCommandService, QueueCommandService>();
            services.AddScoped<IQueueMessageBusService, QueueMessageBusService>();
            services.AddScoped<IQueueHubPayloadService, QueueHubPayloadService>();
            services.AddScoped<IQueueCacheService, QueueCacheService>();

            return services;
        }
    }
}
