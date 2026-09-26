using FeedbackSvc.BLL.Interfaces;
using FeedbackSvc.BLL.Interfaces.MachineLearning;
using MassTransit;
using MessageBroker.Events.FeedbackEvents;
using SharedLibrary.Base.MessageBuses;

namespace FeedbackSvc.WebAPI.Consumers
{
    public class PredictFeedbackRespondToxicConsumer(
        IFeedbackPredictionApiService feedbackPredictionApiService,
        IFeedbackRespondService feedbackRespondService)
        : EventConsumer<PredictFeedbackRespondToxicEvent>
    {
        private readonly IFeedbackPredictionApiService _feedbackPredictionApiService = feedbackPredictionApiService;
        private readonly IFeedbackRespondService _feedbackRespondService = feedbackRespondService;

        protected override async Task Handle(ConsumeContext<PredictFeedbackRespondToxicEvent> context)
        {
            var feedbackRespondId = context.Message.Id;
            var feedbackRespondContent = context.Message.Content;
            if (string.IsNullOrWhiteSpace(feedbackRespondContent))
            {
                return;
            }

            var isToxic = await _feedbackPredictionApiService.GetFeedbackToxicPredictionAsync(feedbackRespondContent);
            if (isToxic)
            {
                await _feedbackRespondService.BanFeedbackRespondAsync(feedbackRespondId);
            }
        }
    }
}
