using AutoMapper;
using EmailSvc.DTOs;
using EmailSvc.Services;
using MassTransit;
using MessageBroker.Events.SendEmailEvents;
using SharedLibrary.Base.MessageBuses;

namespace EmailSvc.Consumers
{
    public class SendVerificationLinkEmailConsumer(IEmailService _emailService, IMapper _mapper) : EventConsumer<SendVerificationLinkEmailEvent>
    {
        protected override async Task Handle(ConsumeContext<SendVerificationLinkEmailEvent> ctx)
        {
            var dto = _mapper.Map<SendLinkEmailDTO>(ctx.Message);
            await _emailService.SendLinkEmailAsync(dto);
        }
    }
}
