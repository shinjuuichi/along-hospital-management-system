using InpatientResourceSvc.BLL.Implements;
using InpatientResourceSvc.BLL.Interfaces;
using InpatientResourceSvc.DAL.Data;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Commons;
using SharedLibrary.Extensions;

namespace InpatientResourceSvc.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, AppConfiguration configuration)
        {
            services.AddApplicationDbContext<InpatientResourceDbContext>(configuration);

            services.AddScoped<IBuildingService, BuildingService>();
            services.AddScoped<IFloorService, FloorService>();
            services.AddScoped<IBedService, BedService>();
            services.AddScoped<IBedCategoryService, BedCategoryService>();
            services.AddScoped<IBedOccupancyService, BedOccupancyService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IRoomCategoryService, RoomCategoryService>();

            services.AddAutoMapper(typeof(InpatientResourceMappingProfile).Assembly);

            return services;
        }
    }
}
