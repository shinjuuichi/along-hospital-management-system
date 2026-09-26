using MessageBroker.Abstractions;

namespace MessageBroker.Events.MedicalOrderEvents
{
    public record UpdateMedicalOrderWhenInvoiceCompletedEvent : BaseEvent
    {
        public string? Id { get; init; }
    }
}