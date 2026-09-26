using BillingSvc.BLL.Interfaces;
using MassTransit;
using MessageBroker.Events.BillingEvents;
using SharedLibrary.Base.MessageBuses;

namespace BillingSvc.WebAPI.Consumers
{
    public class CancelPendingInvoicesByClinicalMedicalOrderIdsConsumer(
        IInvoiceService invoiceService)
            : EventConsumer<CancelPendingInvoicesByClinicalMedicalOrderIdsEvent>
    {
        private readonly IInvoiceService _invoiceService = invoiceService;

        protected override async Task Handle(ConsumeContext<CancelPendingInvoicesByClinicalMedicalOrderIdsEvent> context)
        {
            await _invoiceService.CancelPendingByClinicalMedicalOrderIdsAsync(context.Message.ClinicalMedicalOrderIds);
        }
    }
}
