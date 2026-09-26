using AutoMapper;
using EmailSvc.DTOs;
using EmailSvc.Services;
using MassTransit;
using MessageBroker.Events.SendEmailEvents;
using SharedLibrary.Base.MessageBuses;

namespace EmailSvc.Consumers
{
    public class SendOtpEmailConsumer(IEmailService _emailService, IMapper _mapper) : EventConsumer<SendOtpEmailEvent>
    {
        protected override async Task Handle(ConsumeContext<SendOtpEmailEvent> ctx)
        {
            var dto = _mapper.Map<SendOtpEmailDTO>(ctx.Message);
            await _emailService.SendOtpEmailAsync(dto);
        }
    }
}
