using AutoMapper;
using EmailSvc.DTOs;
using EmailSvc.Services;
using MassTransit;
using MessageBroker.Events.SendEmailEvents;
using SharedLibrary.Base.MessageBuses;

namespace EmailSvc.Consumers
{
    public class SendAppointmentReminderEmailConsumer(IEmailService _emailService, IMapper _mapper) : EventConsumer<SendAppointmentReminderEmailEvent>
    {
        protected override async Task Handle(ConsumeContext<SendAppointmentReminderEmailEvent> ctx)
        {
            var dto = _mapper.Map<SendAppointmentReminderEmailDTO>(ctx.Message);
            await _emailService.SendAppointmentReminderEmailAsync(dto);
        }
    }
}
