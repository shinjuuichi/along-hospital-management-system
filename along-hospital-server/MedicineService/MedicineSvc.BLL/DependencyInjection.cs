using MedicineSvc.BLL.Implements;
using MedicineSvc.BLL.Interfaces;
using MedicineSvc.DAL.Data;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Commons;
using SharedLibrary.Extensions;

namespace MedicineSvc.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, AppConfiguration configuration)
        {
            services.AddApplicationDbContext<MedicineDbContext>(configuration);
            services.AddAutoMapper(typeof(MedicineMappingProfile).Assembly);
            services.AddScoped<IMedicineService, MedicineService>();
            services.AddScoped<IMedicineCategoryService, MedicineCategoryService>();
            services.AddScoped<IMedicineUnitService, MedicineUnitService>();
            services.AddScoped<IOptionService, OptionService>();
            services.AddScoped<IOptionValueService, OptionValueService>();
            services.AddScoped<IMedicineSKUService, MedicineSKUService>();
            services.AddScoped<IMedicineUnitOptionService, MedicineUnitOptionService>();

            return services;
        }
    }
}
