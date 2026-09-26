using MessageBroker.Abstractions;

namespace MessageBroker.Events.PaymentEvents
{
    public record PaymentStatusChangedEvent : BaseEvent
    {
        public Guid TransactionId { get; init; }

        public string PaymentStatus { get; init; } = string.Empty;
    }
}