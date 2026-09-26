using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Commons;
using SharedLibrary.Extensions;
using UserSvc.BLL.Implements;
using UserSvc.BLL.Interfaces;
using UserSvc.DAL.Data;
namespace UserSvc.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, AppConfiguration configuration)
        {
            services.AddApplicationDbContext<UserDbContext>(configuration);
            services.AddRedisDatabase(configuration);

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserStatisticsService, UserStatisticsService>();

            services.AddAutoMapper(typeof(UserMappingProfile).Assembly);
            return services;
        }
    }
}
