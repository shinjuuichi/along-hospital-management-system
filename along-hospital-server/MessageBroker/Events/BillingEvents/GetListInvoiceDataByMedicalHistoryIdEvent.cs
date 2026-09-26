using MessageBroker.Abstractions;

namespace MessageBroker.Events.BillingEvents
{
    public record GetListInvoiceDataByMedicalHistoryIdEvent : BaseEvent
    {
        public int MedicalHistoryId { get; init; }
    }
}