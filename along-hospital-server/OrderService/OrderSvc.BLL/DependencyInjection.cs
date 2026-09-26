using Microsoft.Extensions.DependencyInjection;
using OrderSvc.BLL.Implements;
using OrderSvc.BLL.Interfaces;
using OrderSvc.DAL.Data;
using SharedLibrary.Commons;
using SharedLibrary.Extensions;

namespace OrderSvc.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, AppConfiguration configuration)
        {
            services.AddApplicationDbContext<OrderDbContext>(configuration);
            services.AddScoped<IOrderService, OrderService>();

            services.AddScoped<IOrderStatisticsService, OrderStatisticsService>();

            services.AddAutoMapper(typeof(OrderMappingProfile).Assembly);

            return services;
        }
    }
}
