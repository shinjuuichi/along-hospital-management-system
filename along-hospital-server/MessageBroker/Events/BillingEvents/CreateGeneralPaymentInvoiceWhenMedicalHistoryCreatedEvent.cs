using MessageBroker.Abstractions;

namespace MessageBroker.Events.BillingEvents
{
    public record CreateGeneralPaymentInvoiceWhenMedicalHistoryCreatedEvent : BaseEvent
    {
        public int MedicalHistoryId { get; init; }

        public string? MedicalHistoryType { get; init; }

        public bool IsPaid { get; init; } = false;
    }

    public record CreateGeneralPaymentInvoiceWhenMedicalHistoryCreatedContract : BaseContract;
}