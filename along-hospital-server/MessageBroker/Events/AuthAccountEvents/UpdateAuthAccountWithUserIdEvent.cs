using MessageBroker.Abstractions;

namespace MessageBroker.Events.AuthAccountEvents
{
    public record UpdateAuthAccountWithUserIdEvent : BaseEvent
    {
        public int UserId { get; init; }
        public int AuthId { get; init; }
        public string? Phone { get; init; }
    }
}
