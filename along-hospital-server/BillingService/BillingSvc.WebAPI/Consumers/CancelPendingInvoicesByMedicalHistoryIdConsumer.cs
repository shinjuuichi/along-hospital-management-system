using BillingSvc.BLL.Interfaces;
using MassTransit;
using MessageBroker.Events.BillingEvents;
using SharedLibrary.Base.MessageBuses;

namespace BillingSvc.WebAPI.Consumers
{
    public class CancelPendingInvoicesByMedicalHistoryIdConsumer(
        IInvoiceService invoiceService)
            : EventConsumer<CancelPendingInvoicesByMedicalHistoryIdEvent>
    {
        private readonly IInvoiceService _invoiceService = invoiceService;

        protected override async Task Handle(ConsumeContext<CancelPendingInvoicesByMedicalHistoryIdEvent> context)
        {
            await _invoiceService.CancelAllPendingByMedicalHistoryIdAsync(context.Message.MedicalHistoryId);
        }
    }
}