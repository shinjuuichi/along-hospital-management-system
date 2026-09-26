using AppointmentSvc.BLL.Implements;
using AppointmentSvc.BLL.Interfaces;
using AppointmentSvc.DAL.Data;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Commons;
using SharedLibrary.Extensions;

namespace AppointmentSvc.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, AppConfiguration configuration)
        {
            services.AddApplicationDbContext<AppointmentDbContext>(configuration);
            services.AddAutoMapper(typeof(AppointmentMappingProfile).Assembly);

            services.AddScoped<IAppointmentQueryService, AppointmentQueryService>();
            services.AddScoped<IAppointmentCommandService, AppointmentCommandService>();
            services.AddScoped<IAppointmentBackgroundService, AppointmentBackgroundService>();
            services.AddScoped<IAppointmentStatisticsService, AppointmentStatisticsService>();
            services.AddScoped<ITimeSlotService, TimeSlotService>();

            return services;
        }
    }
}
