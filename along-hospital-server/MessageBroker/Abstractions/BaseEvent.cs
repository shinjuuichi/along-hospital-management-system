using MassTransit;

namespace MessageBroker.Abstractions
{
    [ExcludeFromTopology]
    public abstract record BaseEvent
    {
        public Guid EventId { get; init; } = Guid.NewGuid();
        public DateTime OccurredAt { get; init; } = DateTime.UtcNow;
    }
}
