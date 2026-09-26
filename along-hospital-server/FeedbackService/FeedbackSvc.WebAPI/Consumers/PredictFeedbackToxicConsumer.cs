using FeedbackSvc.BLL.Interfaces;
using FeedbackSvc.BLL.Interfaces.MachineLearning;
using MassTransit;
using MessageBroker.Events.FeedbackEvents;
using SharedLibrary.Base.MessageBuses;

namespace FeedbackSvc.WebAPI.Consumers
{
    public class PredictFeedbackToxicConsumer(
        IFeedbackPredictionApiService feedbackPredictionApiService,
        IFeedbackService feedbackService)
        : EventConsumer<PredictFeedbackToxicEvent>
    {
        private readonly IFeedbackPredictionApiService _feedbackPredictionApiService = feedbackPredictionApiService;
        private readonly IFeedbackService _feedbackService = feedbackService;

        protected override async Task Handle(ConsumeContext<PredictFeedbackToxicEvent> context)
        {
            var feedbackId = context.Message.Id;
            var feedbackText = context.Message.Content;
            if (string.IsNullOrWhiteSpace(feedbackText))
            {
                return;
            }

            var isToxic = await _feedbackPredictionApiService.GetFeedbackToxicPredictionAsync(feedbackText);
            if (isToxic)
            {
                await _feedbackService.BanFeedbackAsync(feedbackId);
            }
        }
    }
}