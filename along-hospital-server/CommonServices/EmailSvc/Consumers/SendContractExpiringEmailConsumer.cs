using AutoMapper;
using EmailSvc.DTOs;
using EmailSvc.Services;
using MassTransit;
using MessageBroker.Events.SendEmailEvents;
using SharedLibrary.Base.MessageBuses;

namespace EmailSvc.Consumers
{
    public class SendContractExpiringEmailConsumer(
        IEmailService emailService,
        IMapper mapper) : EventConsumer<SendContractExpiringEmailEvent>
    {
        protected override async Task Handle(ConsumeContext<SendContractExpiringEmailEvent> ctx)
        {
            var dto = mapper.Map<SendContractExpiringEmailDTO>(ctx.Message);
            await emailService.SendContractExpiringEmailAsync(dto);
        }
    }
}
