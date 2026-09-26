using InventorySvc.BLL.Implements;
using InventorySvc.BLL.Interfaces;
using InventorySvc.DAL.Data;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Commons;
using SharedLibrary.Extensions;

namespace InventorySvc.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, AppConfiguration configuration)
        {
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<IInventoryStatisticsService, InventoryStatisticsService>();
            services.AddAutoMapper(typeof(InventoryMappingProfile).Assembly);
            services.AddApplicationDbContext<InventoryDbContext>(configuration);
            return services;
        }
    }
}