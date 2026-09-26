using AutoMapper;
using EmailSvc.DTOs;
using EmailSvc.Services;
using MassTransit;
using MessageBroker.Events.SendEmailEvents;
using SharedLibrary.Base.MessageBuses;

namespace EmailSvc.Consumers
{
    public class SendCertificateExpirationReminderEmailConsumer(IEmailService _emailService, IMapper _mapper)
        : EventConsumer<SendCertificateExpirationReminderEmailEvent>
    {
        protected override async Task Handle(ConsumeContext<SendCertificateExpirationReminderEmailEvent> ctx)
        {
            var dto = _mapper.Map<SendCertificateExpirationReminderEmailDTO>(ctx.Message);
            await _emailService.SendCertificateExpirationReminderEmailAsync(dto);
        }
    }
}
