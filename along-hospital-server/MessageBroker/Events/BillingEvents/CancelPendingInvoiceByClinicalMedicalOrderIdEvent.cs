using MessageBroker.Abstractions;

namespace MessageBroker.Events.BillingEvents
{
    public record CancelPendingInvoiceByClinicalMedicalOrderIdEvent : BaseEvent
    {
        public string? ClinicalMedicalOrderId { get; init; }
    }

    public record CancelPendingInvoicesByClinicalMedicalOrderIdsEvent : BaseEvent
    {
        public List<string> ClinicalMedicalOrderIds { get; init; } = [];
    }
}