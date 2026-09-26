using AutoMapper;
using BillingSvc.BLL.DTOs.InvoiceDTOs;
using BillingSvc.BLL.Interfaces;
using MassTransit;
using MessageBroker.Events.PaymentEvents;
using SharedLibrary.Base.MessageBuses;

namespace BillingSvc.WebAPI.Consumers
{
    public class BillingPaymentStatusChangedConsumer(
        IInvoiceService invoiceService,
        IMapper mapper)
            : EventConsumer<PaymentStatusChangedEvent>
    {
        private readonly IInvoiceService _invoiceService = invoiceService;
        private readonly IMapper _mapper = mapper;

        protected override async Task Handle(ConsumeContext<PaymentStatusChangedEvent> context)
        {
            var paymentStatusChangedDTO = _mapper.Map<PaymentStatusChangedDTO>(context.Message);
            await _invoiceService.HandlePaymentStatusChangedAsync(paymentStatusChangedDTO);
        }
    }
}
