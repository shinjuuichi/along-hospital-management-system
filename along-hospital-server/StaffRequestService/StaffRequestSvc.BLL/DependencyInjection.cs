using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Commons;
using SharedLibrary.Extensions;
using StaffRequestSvc.BLL.Implements;
using StaffRequestSvc.BLL.Interfaces;
using StaffRequestSvc.DAL.Data;

namespace StaffRequestSvc.BLL;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, AppConfiguration configuration)
    {
        services.AddApplicationDbContext<StaffRequestDbContext>(configuration);

        services.AddScoped<ILeaveRequestService, LeaveRequestService>();
        services.AddScoped<ISalaryAdvanceService, SalaryAdvanceService>();
        services.AddScoped<IStaffRequestStatisticsService, StaffRequestStatisticsService>();

        services.AddAutoMapper(typeof(StaffRequestMappingProfile).Assembly);

        return services;
    }
}
