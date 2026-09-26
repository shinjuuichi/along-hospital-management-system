using MessageBroker.Abstractions;

namespace MessageBroker.Events.AuthAccountEvents.GetUserDataEvents
{
    public record GetUserRoleByUserIdEvent : BaseEvent
    {
        public int Id { get; init; }
    }
}