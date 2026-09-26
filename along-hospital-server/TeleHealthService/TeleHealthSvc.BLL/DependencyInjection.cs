using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Commons;
using SharedLibrary.Extensions;
using TeleHealthSvc.BLL.Implements;
using TeleHealthSvc.BLL.Interfaces;
using TeleHealthSvc.DAL.Data;

namespace TeleHealthSvc.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, AppConfiguration configuration)
        {
            services.AddApplicationDbContext<TeleHealthDbContext>(configuration);

            services.AddScoped<ITeleRoomService, TeleRoomService>();
            services.AddScoped<ITeleSessionService, TeleSessionService>();

            services.AddAutoMapper(typeof(TeleHealthMappingProfile).Assembly);

            return services;
        }
    }
}