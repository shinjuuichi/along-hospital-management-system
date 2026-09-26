using AutoMapper;
using EmailSvc.DTOs;
using EmailSvc.Services;
using MassTransit;
using MessageBroker.Events.SendEmailEvents;
using SharedLibrary.Base.MessageBuses;

namespace EmailSvc.Consumers
{
    public class SendInterviewEmailConsumer(IEmailService _emailService, IMapper _mapper) : EventConsumer<SendInterviewEmailEvent>
    {
        protected override async Task Handle(ConsumeContext<SendInterviewEmailEvent> ctx)
        {
            var interviewEmailDto = _mapper.Map<SendInterviewEmailDTO>(ctx.Message);
            await _emailService.SendInterviewEmailAsync(interviewEmailDto);
        }
    }
}