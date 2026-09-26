using BillingSvc.BLL.Interfaces;
using MassTransit;
using MessageBroker.Events.BillingEvents;
using SharedLibrary.Base.MessageBuses;

namespace BillingSvc.WebAPI.Consumers
{
    public class CancelPendingInvoiceByClinicalMedicalOrderIdConsumer(
        IInvoiceService invoiceService)
            : EventConsumer<CancelPendingInvoiceByClinicalMedicalOrderIdEvent>
    {
        private readonly IInvoiceService _invoiceService = invoiceService;

        protected override async Task Handle(ConsumeContext<CancelPendingInvoiceByClinicalMedicalOrderIdEvent> context)
        {
            var clinicalMedicalOrderId = context.Message.ClinicalMedicalOrderId;
            if (string.IsNullOrEmpty(clinicalMedicalOrderId))
            {
                return;
            }

            await _invoiceService.CancelPendingByClinicalMedicalOrderIdAsync(clinicalMedicalOrderId);
        }
    }
}