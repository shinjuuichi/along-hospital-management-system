using MessageBroker.Abstractions;

namespace MessageBroker.Events.OrderEvents
{
    public record UpdateOrderStatusEvent : BaseEvent
    {
        public Guid TransactionId { get; init; }
    }
}