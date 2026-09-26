using Microsoft.Extensions.DependencyInjection;
using ReportSvc.BLL.Implements;
using ReportSvc.BLL.Implements.ExternalServices;
using ReportSvc.BLL.Interfaces;
using ReportSvc.BLL.Interfaces.ExternalServices;
using SharedLibrary.Commons;
using SharedLibrary.Services.Implements;
using SharedLibrary.Services.Interfaces;

namespace ReportSvc.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, AppConfiguration configuration)
        {
            services.AddScoped<IManagerDashboardService, ManagerDashboardService>();
            services.AddScoped<IAccountantDashboardService, AccountantDashboardService>();
            services.AddScoped<IInventoryClerkDashboardService, InventoryClerkDashboardService>();
            services.AddScoped<IPharmacistDashboardService, PharmacistDashboardService>();
            services.AddScoped<IHrDashboardService, HrDashboardService>();
            services.AddScoped<IExternalOrderStatisticsService, ExternalOrderStatisticsService>();
            services.AddScoped<IExternalBillingStatisticsService, ExternalBillingStatisticsService>();
            services.AddScoped<IExternalMedicalHistoryStatisticsService, ExternalMedicalHistoryStatisticsService>();
            services.AddScoped<IExternalAppointmentStatisticsService, ExternalAppointmentStatisticsService>();
            services.AddScoped<IExternalStaffRequestStatisticsService, ExternalStaffRequestStatisticsService>();
            services.AddScoped<IExternalRecruitmentStatisticsService, ExternalRecruitmentStatisticsService>();
            services.AddScoped<IExternalUserGrowthStatisticsService, ExternalUserGrowthStatisticsService>();
            services.AddScoped<IExternalInventoryStatisticsService, ExternalInventoryStatisticsService>();
            services.AddScoped<IExternalSupplierStatisticsService, ExternalSupplierStatisticsService>();
            services.AddScoped<IExcelService, ExcelService>();

            services.AddAutoMapper(typeof(ReportMappingProfile).Assembly);

            return services;
        }
    }
}
