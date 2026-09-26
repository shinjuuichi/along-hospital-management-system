using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Commons;
using SharedLibrary.Extensions;
using System.Reflection;
using VoucherSvc.BLL.Implements;
using VoucherSvc.BLL.Interfaces;
using VoucherSvc.DAL.Data;

namespace VoucherSvc.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, AppConfiguration configuration, Assembly? consumersAssembly = null)
        {
            services.AddMongoDatabase<VoucherDbContext>(configuration);

            services.AddScoped<IPatientVoucherService, PatientVoucherService>();
            services.AddScoped<IVoucherService, VoucherService>();
            services.AddScoped<IDiscountService, DiscountService>();

            services.AddAutoMapper(typeof(VoucherMappingProfile).Assembly);

            return services;
        }
    }
}
