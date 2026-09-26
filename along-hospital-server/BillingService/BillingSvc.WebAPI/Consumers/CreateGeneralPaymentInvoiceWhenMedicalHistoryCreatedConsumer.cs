using BillingSvc.BLL.DTOs.InvoiceDTOs;
using BillingSvc.BLL.Interfaces;
using MassTransit;
using MessageBroker.Events.BillingEvents;
using SharedLibrary.Base.MessageBuses;

namespace BillingSvc.WebAPI.Consumers
{
    public class CreateGeneralPaymentInvoiceWhenMedicalHistoryCreatedConsumer(
        IInvoiceService invoiceService)
        : RequestConsumer<CreateGeneralPaymentInvoiceWhenMedicalHistoryCreatedEvent,
            CreateGeneralPaymentInvoiceWhenMedicalHistoryCreatedContract>
    {
        private readonly IInvoiceService _invoiceService = invoiceService;

        protected override async Task<CreateGeneralPaymentInvoiceWhenMedicalHistoryCreatedContract> Handle(
            ConsumeContext<CreateGeneralPaymentInvoiceWhenMedicalHistoryCreatedEvent> context)
        {
            var medicalHistoryId = context.Message.MedicalHistoryId;
            var medicalHistoryType = context.Message.MedicalHistoryType;
            var isPaid = context.Message.IsPaid;

            var createGeneralInvoiceDTO = new CreateGeneralInvoiceDTO
            {
                MedicalHistoryId = medicalHistoryId,
                MedicalHistoryType = medicalHistoryType,
                IsPaid = isPaid,
            };

            await _invoiceService.CreateGeneralInvoiceAsync(createGeneralInvoiceDTO);
            return new();
        }
    }
}
