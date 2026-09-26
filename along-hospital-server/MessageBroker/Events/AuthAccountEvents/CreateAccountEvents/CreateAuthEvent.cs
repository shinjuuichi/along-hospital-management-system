using MessageBroker.Abstractions;

namespace MessageBroker.Events.AuthAccountEvents.CreateAccountEvents
{
    public record CreateAuthEvent : BaseEvent
    {
        public string? Email { get; init; }

        public string? Phone { get; init; }

        public int UserId { get; init; }
    }
}
