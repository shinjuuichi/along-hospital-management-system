using AutoMapper;
using EmailSvc.DTOs;
using EmailSvc.Services;
using MassTransit;
using MessageBroker.Events.SendEmailEvents;
using SharedLibrary.Base.MessageBuses;

namespace EmailSvc.Consumers
{
    public class SendInvoiceEmailConsumer(IEmailService _emailService, IMapper _mapper) : EventConsumer<SendInvoiceEmailEvent>
    {
        protected override async Task Handle(ConsumeContext<SendInvoiceEmailEvent> ctx)
        {
            var dto = _mapper.Map<SendInvoiceEmailDTO>(ctx.Message);
            await _emailService.SendInvoiceEmailAsync(dto);
        }
    }
}