using MessageBroker.Abstractions;

namespace MessageBroker.Events.UserEvents
{
    public record GetListUserDataByRoleEvent : BaseEvent
    {
        public string? Role { get; init; }
    }
}