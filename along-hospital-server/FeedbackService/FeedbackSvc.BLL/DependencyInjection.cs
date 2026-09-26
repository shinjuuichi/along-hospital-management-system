using FeedbackSvc.BLL.Implements;
using FeedbackSvc.BLL.Implements.MachineLearning;
using FeedbackSvc.BLL.Implements.Management;
using FeedbackSvc.BLL.Interfaces;
using FeedbackSvc.BLL.Interfaces.MachineLearning;
using FeedbackSvc.BLL.Interfaces.Management;
using FeedbackSvc.DAL.Data;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Commons;
using SharedLibrary.Extensions;

namespace FeedbackSvc.BLL
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, AppConfiguration configuration)
        {
            services.AddApplicationDbContext<FeedbackDbContext>(configuration);

            services.AddAutoMapper(typeof(FeedbackMappingProfile).Assembly);

            services.AddScoped<IFeedbackService, FeedbackService>();
            services.AddScoped<IFeedbackReportService, FeedbackReportService>();
            services.AddScoped<IFeedbackRespondService, FeedbackRespondService>();
            services.AddScoped<IFeedbackManagementService, FeedbackManagementService>();

            services.AddHttpClient<IFeedbackPredictionApiService, FeedbackPredictionApiService>(client =>
            {
                client.BaseAddress = new Uri(configuration.ApiUrlsConfig.MachineLearningUrl);
            });

            return services;
        }
    }
}