using MessageBroker.Abstractions;

namespace MessageBroker.Events.CartEvents
{
    public record CreateCartEvent : BaseEvent
    {
        public int PatientId { get; init; }
    }

    public record CreateCartContract : BaseContract;
}
