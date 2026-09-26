using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Commons;
using SharedLibrary.Extensions;
using SharedLibrary.Services.Implements;
using SharedLibrary.Services.Interfaces;
using SupplierSvc.BLL.Implements;
using SupplierSvc.BLL.Interfaces;
using SupplierSvc.DAL.Data;

namespace SupplierSvc.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, AppConfiguration configuration)
        {
            services.AddApplicationDbContext<SupplierDbContext>(configuration);
            services.AddScoped<IExcelService, ExcelService>();
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<IImportService, ImportService>();
            services.AddScoped<IImportRequestService, ImportRequestService>();
            services.AddScoped<IImportExcelService, ImportExcelService>();
            services.AddScoped<ISupplierStatisticsService, SupplierStatisticsService>();
            services.AddScoped<IImportStatisticsService, ImportStatisticsService>();

            services.AddAutoMapper(typeof(DependencyInjection).Assembly);
            return services;
        }
    }
}