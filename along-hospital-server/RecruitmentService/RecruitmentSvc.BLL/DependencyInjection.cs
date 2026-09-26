using Microsoft.Extensions.DependencyInjection;
using RecruitmentSvc.BLL.Implements;
using RecruitmentSvc.BLL.Interfaces;
using RecruitmentSvc.DAL.Data;
using SharedLibrary.Commons;
using SharedLibrary.Extensions;

namespace RecruitmentSvc.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, AppConfiguration configuration)
        {
            services.AddApplicationDbContext<RecruitmentDbContext>(configuration);
            services.AddScoped<IJobPostingService, JobPostingService>();
            services.AddScoped<IJobApplicationService, JobApplicationService>();
            services.AddScoped<IRecruitmentStatisticsService, RecruitmentStatisticsService>();
            services.AddScoped<IInterviewService, InterviewService>();
            services.AddScoped<IInterviewTypeService, InterviewTypeService>();
            services.AddAutoMapper(typeof(DependencyInjection).Assembly);
            return services;
        }
    }
}
