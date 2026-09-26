using MassTransit;
using MessageBroker.Events.SendSmsEvents;
using SharedLibrary.Base.MessageBuses;
using SmsSvc.Services;

namespace SmsSvc.Consumers
{
    public class SendResetPasswordLinkSmsConsumer(ISmsService _smsService) : EventConsumer<SendResetPasswordLinkSmsEvent>
    {
        protected override async Task Handle(ConsumeContext<SendResetPasswordLinkSmsEvent> ctx)
        {
            await _smsService.SendLinkAsync(ctx.Message.PhoneNumber, ctx.Message.Link);
        }
    }
}
