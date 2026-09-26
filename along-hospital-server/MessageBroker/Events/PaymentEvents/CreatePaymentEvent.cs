using MessageBroker.Abstractions;

namespace MessageBroker.Events.PaymentEvents
{
    public record CreatePaymentEvent : BaseEvent
    {
        public string? Description { get; init; }

        public string? Provider { get; init; }

        public List<PaymentEventItem> PaymentEventItems { get; init; } = [];
    }

    public record PaymentEventItem
    {
        public string? ServiceName { get; init; }

        public int Quantity { get; init; }

        public double UnitPrice { get; init; }
    }
}