using MassTransit;
using MessageBroker.Events.SendSmsEvents;
using SharedLibrary.Base.MessageBuses;
using SmsSvc.Services;

namespace SmsSvc.Consumers
{
    public class SendOtpSmsConsumer(ISmsService _smsService) : EventConsumer<SendOtpSmsEvent>
    {
        protected override async Task Handle(ConsumeContext<SendOtpSmsEvent> ctx)
        {
            await _smsService.SendOtpAsync(ctx.Message.PhoneNumber, ctx.Message.Otp);
        }
    }
}
