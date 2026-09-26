using AutoMapper;
using EmailSvc.DTOs;
using EmailSvc.Services;
using MassTransit;
using MessageBroker.Events.SendEmailEvents;
using SharedLibrary.Base.MessageBuses;

namespace EmailSvc.Consumers
{
    public class SendFeedbackRespondEmailConsumer(IEmailService _emailService, IMapper _mapper) : EventConsumer<SendFeedbackRespondEvent>
    {
        protected override async Task Handle(ConsumeContext<SendFeedbackRespondEvent> ctx)
        {
            var dto = _mapper.Map<SendFeedbackRespondEmailDTO>(ctx.Message);
            await _emailService.SendFeedbackRespondEmailAsync(dto);
        }
    }
}
