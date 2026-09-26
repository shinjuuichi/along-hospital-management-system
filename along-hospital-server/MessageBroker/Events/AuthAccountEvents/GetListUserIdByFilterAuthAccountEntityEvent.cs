using MessageBroker.Abstractions;

namespace MessageBroker.Events.AuthAccountEvents
{
    public record GetListUserIdByFilterAuthAccountEntityEvent : BaseEvent
    {
        public string? Phone { get; init; }

        public string? Email { get; init; }
    }
}