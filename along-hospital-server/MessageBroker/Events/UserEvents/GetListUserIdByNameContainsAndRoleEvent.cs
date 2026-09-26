using MessageBroker.Abstractions;

namespace MessageBroker.Events.UserEvents
{
    public record GetListUserIdByNameContainsAndRoleEvent : BaseEvent
    {
        public string? Name { get; init; }

        public string? Role { get; init; }
    }
}