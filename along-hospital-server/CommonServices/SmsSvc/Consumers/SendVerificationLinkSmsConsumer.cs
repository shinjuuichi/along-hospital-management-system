using MassTransit;
using MessageBroker.Events.SendSmsEvents;
using SharedLibrary.Base.MessageBuses;
using SmsSvc.Services;

namespace SmsSvc.Consumers
{
    public class SendVerificationLinkSmsConsumer(ISmsService _smsService) : EventConsumer<SendVerificationLinkSmsEvent>
    {
        protected override async Task Handle(ConsumeContext<SendVerificationLinkSmsEvent> ctx)
        {
            await _smsService.SendLinkAsync(ctx.Message.PhoneNumber, ctx.Message.Link);
        }
    }
}
