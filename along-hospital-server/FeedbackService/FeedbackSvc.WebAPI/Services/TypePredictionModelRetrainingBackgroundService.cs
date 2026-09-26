using FeedbackSvc.BLL.Interfaces.MachineLearning;
using SharedLibrary.Base.Services;

namespace FeedbackSvc.WebAPI.Services
{
    public class TypePredictionModelRetrainingBackgroundService(IServiceProvider serviceProvider)
        : BaseBackgroundService(serviceProvider)
    {
        protected override async Task ExecuteTaskAsync(IServiceScope scope, CancellationToken cancellationToken)
        {
            var feedbackPredictionApiService = this.GetService<IFeedbackPredictionApiService>(scope);
            await feedbackPredictionApiService.RetrainFeedbackTypeModelAsync();
            await feedbackPredictionApiService.RetrainFeedbackToxicModelAsync();
        }

        protected override TimeSpan GetInterval()
        {
            return TimeSpan.FromDays(30);
        }
    }
}