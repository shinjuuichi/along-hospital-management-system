using AttendanceSvc.BLL.Implements;
using AttendanceSvc.BLL.Interfaces;
using AttendanceSvc.DAL.Data;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Commons;
using SharedLibrary.Extensions;

namespace AttendanceSvc.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, AppConfiguration configuration)
        {
            services.AddApplicationDbContext<AttendanceDbContext>(configuration);
            services.AddAutoMapper(typeof(AttendanceMappingProfile).Assembly);

            services.AddScoped<IAttendanceService, AttendanceService>();
            services.AddHttpClient<IStaffRecognizationApiService, StaffRecognizationApiService>(client =>
            {
                client.BaseAddress = new Uri(configuration.ApiUrlsConfig.MachineLearningUrl);
            });

            return services;
        }
    }
}
