using Microsoft.Extensions.DependencyInjection;
using PayrollSvc.BLL.Implements;
using PayrollSvc.BLL.Interfaces;
using PayrollSvc.DAL.Data;
using SharedLibrary.Commons;
using SharedLibrary.Extensions;

namespace PayrollSvc.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, AppConfiguration configuration)
        {
            services.AddApplicationDbContext<PayrollDbContext>(configuration);

            services.AddAutoMapper(typeof(PayrollMappingProfile).Assembly);

            services.AddScoped<IDeductionService, DeductionService>();
            services.AddScoped<IDeductionTypeService, DeductionTypeService>();
            services.AddScoped<IAllowanceService, AllowanceService>();
            services.AddScoped<IAllowanceTypeService, AllowanceTypeService>();
            services.AddScoped<IPayrollService, PayrollService>();
            services.AddScoped<IGlobalTaxConfigService, GlobalTaxConfigService>();
            services.AddScoped<IPayrollPolicyService, PayrollPolicyManagementService>();
            services.AddScoped<ITaxBracketService, TaxBracketService>();

            return services;
        }
    }
}
