using ChatboxSvc.WebAPI.Implements;
using ChatboxSvc.WebAPI.Interfaces;
using GenerativeAI;
using GenerativeAI.Microsoft;
using Microsoft.Extensions.AI;
using Qdrant.Client;
using SharedLibrary.Commons;
using SharedLibrary.Extensions;

namespace ChatboxSvc.WebAPI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, AppConfiguration configuration)
        {
            services.AddSignalR();
            services.AddRedisDatabase(configuration);

            services.AddSingleton<QdrantClient>(sp => new(configuration.QdrantConfig.Host, configuration.QdrantConfig.Port));
            services.AddSingleton<IChatClient>(sp =>
            {
                var apiKey = configuration.GoogleConfig.GeminiAPIKey;
                return new GenerativeAIChatClient(apiKey, GoogleAIModels.Gemini25FlashLite);
            });

            services.AddScoped<IAnalyticService, AnalyticService>();
            services.AddScoped<IChatBotService, ChatBotService>();
            services.AddScoped<IRagIndexBuilderService, RagIndexBuilderService>();
            services.AddSingleton<IVectorSearchService, QdrantVectorSearchService>();
            services.AddHttpClient<IEmbeddingService, PythonEmbeddingApiService>(client =>
            {
                client.BaseAddress = new Uri(configuration.ApiUrlsConfig.MachineLearningUrl);
            });

            return services;
        }
    }
}
