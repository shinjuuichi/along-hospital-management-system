using MessageBroker.Abstractions;

namespace MessageBroker.Events.BillingEvents
{
    public record CancelPendingInvoicesByMedicalHistoryIdEvent : BaseEvent
    {
        public int MedicalHistoryId { get; init; }
    }
}