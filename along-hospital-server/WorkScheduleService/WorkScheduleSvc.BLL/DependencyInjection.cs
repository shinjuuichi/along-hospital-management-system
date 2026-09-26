using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Commons;
using SharedLibrary.Extensions;
using WorkScheduleSvc.BLL.Implements;
using WorkScheduleSvc.BLL.Implements.Calculators;
using WorkScheduleSvc.BLL.Implements.Gateways;
using WorkScheduleSvc.BLL.Implements.QueryServices;
using WorkScheduleSvc.BLL.Implements.Validators;
using WorkScheduleSvc.BLL.Interfaces;
using WorkScheduleSvc.BLL.Interfaces.Calculators;
using WorkScheduleSvc.BLL.Interfaces.Gateways;
using WorkScheduleSvc.BLL.Interfaces.Querys;
using WorkScheduleSvc.BLL.Interfaces.Validators;
using WorkScheduleSvc.DAL.Data;

namespace WorkScheduleSvc.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, AppConfiguration configuration)
        {
            services.AddApplicationDbContext<WorkScheduleDbContext>(configuration);

            services.AddAutoMapper(typeof(WorkScheduleMappingProfile).Assembly);

            services.AddScoped<IHolidayService, HolidayService>();
            services.AddScoped<IShiftService, ShiftService>();
            services.AddScoped<IWorkScheduleService, WorkScheduleService>();
            services.AddScoped<IWorkScheduleAssignmentService, WorkScheduleAssignmentService>();
            services.AddScoped<IWorkSegmentService, WorkSegmentService>();
            services.AddScoped<IWorkScheduleTemplateService, WorkScheduleTemplateService>();
            services.AddScoped<IWorkScheduleTemplateDayShiftService, WorkScheduleTemplateDayShiftService>();
            services.AddScoped<IWorkScheduleTemplateRoomAssignmentService, WorkScheduleTemplateRoomAssignmentService>();
            services.AddScoped<IWorkScheduleTemplateTeleRoomAssignmentService, WorkScheduleTemplateTeleRoomAssignmentService>();

            services.AddScoped<IWorkScheduleAssignmentGateway, WorkScheduleAssignmentGateway>();
            services.AddScoped<IWorkSegmentGateway, WorkSegmentGateway>();

            services.AddScoped<IWorkScheduleAssignmentValidator, WorkScheduleAssignmentValidator>();
            services.AddScoped<IWorkScheduleValidator, WorkScheduleValidator>();
            services.AddScoped<IWorkSegmentValidator, WorkSegmentValidator>();

            services.AddScoped<IWorkScheduleAssignmentQueryService, WorkScheduleAssignmentQueryService>();
            services.AddScoped<IWorkScheduleQueryService, WorkScheduleQueryService>();
            services.AddScoped<IWorkSegmentQueryService, WorkSegmentQueryService>();

            services.AddScoped<IWorkSegmentCalculator, WorkSegmentCalculator>();

            services.AddScoped<WorkScheduleAssignmentDetailsQueryHelper>();

            return services;
        }
    }
}
