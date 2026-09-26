using FeedbackSvc.BLL.Interfaces;
using MassTransit;
using MessageBroker.Events.FeedbackEvents;
using SharedLibrary.Base.MessageBuses;

namespace FeedbackSvc.WebAPI.Consumers
{
    public class BanFeedbackConsumer(IFeedbackService feedbackService) : EventConsumer<BanFeedbackEvent>
    {
        private readonly IFeedbackService _feedbackService = feedbackService;

        protected override async Task Handle(ConsumeContext<BanFeedbackEvent> context)
        {
            await _feedbackService.BanFeedbackByUserIdAsync(context.Message.UserId);
        }
    }
}