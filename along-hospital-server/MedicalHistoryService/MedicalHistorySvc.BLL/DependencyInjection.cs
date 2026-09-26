using MedicalHistorySvc.BLL.Implements;
using MedicalHistorySvc.BLL.Interfaces;
using MedicalHistorySvc.DAL.Data;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Commons;
using SharedLibrary.Extensions;

namespace MedicalHistorySvc.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, AppConfiguration configuration)
        {
            services.AddApplicationDbContext<MedicalHistoryDbContext>(configuration);
            services.AddAutoMapper(typeof(MedicalHistoryMappingProfile).Assembly);
            services.AddScoped<IComplaintService, ComplaintService>();
            services.AddScoped<IMedicalHistoryQueryService, MedicalHistoryQueryService>();
            services.AddScoped<IMedicalHistoryCommandService, MedicalHistoryCommandService>();
            services.AddScoped<IMedicalHistoryStatisticsService, MedicalHistoryStatisticsService>();
            services.AddScoped<IPrescriptionService, PrescriptionService>();
            services.AddHttpClient<IComplaintPredictionApiService, ComplaintPredictionApiService>(client =>
            {
                client.BaseAddress = new Uri(configuration.ApiUrlsConfig.MachineLearningUrl);
            });
            return services;
        }
    }
}
