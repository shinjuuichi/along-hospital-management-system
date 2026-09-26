using MessageBroker.Abstractions;

namespace MessageBroker.Events.AuthAccountEvents.UpdateAccountEvents
{
    public record UpdateAuthEvent : BaseEvent
    {
        public string? Email { get; init; }

        public string? Phone { get; init; }

        public int UserId { get; init; }
    }
}
