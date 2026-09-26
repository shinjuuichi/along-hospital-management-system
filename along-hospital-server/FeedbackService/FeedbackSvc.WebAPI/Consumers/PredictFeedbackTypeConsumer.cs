using FeedbackSvc.BLL.Interfaces;
using FeedbackSvc.BLL.Interfaces.MachineLearning;
using MassTransit;
using MessageBroker.Events.FeedbackEvents;
using SharedLibrary.Base.MessageBuses;

namespace FeedbackSvc.WebAPI.Consumers
{
    public class PredictFeedbackTypeConsumer(
        IFeedbackPredictionApiService feedbackPredictionApiService,
        IFeedbackService feedbackService)
        : EventConsumer<PredictFeedbackTypeEvent>
    {
        private readonly IFeedbackPredictionApiService _feedbackPredictionApiService = feedbackPredictionApiService;
        private readonly IFeedbackService _feedbackService = feedbackService;

        protected override async Task Handle(ConsumeContext<PredictFeedbackTypeEvent> context)
        {
            var feedbackId = context.Message.Id;
            var feedbackText = context.Message.Content;
            if (string.IsNullOrWhiteSpace(feedbackText))
            {
                return;
            }

            var feedbackType = await _feedbackPredictionApiService.GetFeedbackTypePredictionAsync(feedbackText);
            await _feedbackService.UpdateFeedbackTypeAsync(feedbackId, feedbackType);
        }
    }
}