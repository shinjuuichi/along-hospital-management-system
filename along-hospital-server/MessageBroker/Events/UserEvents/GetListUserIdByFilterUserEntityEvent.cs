using MessageBroker.Abstractions;

namespace MessageBroker.Events.UserEvents
{
    public record GetListUserIdByFilterUserEntityEvent : BaseEvent
    {
        public string? Role { get; init; }

        public string? Name { get; init; }

        public string? Gender { get; init; }
    }
}