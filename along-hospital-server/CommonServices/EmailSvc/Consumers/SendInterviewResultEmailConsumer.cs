using AutoMapper;
using EmailSvc.DTOs;
using EmailSvc.Services;
using MassTransit;
using MessageBroker.Events.SendEmailEvents;
using SharedLibrary.Base.MessageBuses;

namespace EmailSvc.Consumers
{
    public class SendInterviewResultEmailConsumer(IEmailService _emailService, IMapper _mapper) : EventConsumer<SendInterviewResultEmailEvent>
    {
        protected override async Task Handle(ConsumeContext<SendInterviewResultEmailEvent> ctx)
        {
            var interviewResultEmailDto = _mapper.Map<SendInterviewResultEmailDTO>(ctx.Message);
            await _emailService.SendInterviewResultEmailAsync(interviewResultEmailDto);
        }
    }
}
