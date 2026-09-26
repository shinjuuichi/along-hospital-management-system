using CartSvc.BLL.Implements;
using CartSvc.BLL.Interfaces;
using CartSvc.DAL.Data;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Commons;
using SharedLibrary.Extensions;

namespace CartSvc.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, AppConfiguration configuration)
        {
            services.AddApplicationDbContext<CartDbContext>(configuration);
            services.AddAutoMapper(typeof(CartMappingProfile).Assembly);
            services.AddScoped<ICartService, CartService>();
            return services;
        }
    }
}
