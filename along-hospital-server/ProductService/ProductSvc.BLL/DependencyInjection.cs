using Microsoft.Extensions.DependencyInjection;
using ProductSvc.BLL.Implements;
using ProductSvc.BLL.Interfaces;
using ProductSvc.DAL.Data;
using SharedLibrary.Commons;
using SharedLibrary.Extensions;

namespace ProductSvc.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, AppConfiguration configuration)
        {
            services.AddApplicationDbContext<ProductDbContext>(configuration);

            services.AddUploadService(configuration);

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();

            services.AddAutoMapper(typeof(ProductMappingProfile).Assembly);

            return services;
        }
    }
}
