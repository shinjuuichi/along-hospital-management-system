using MessageBroker.Abstractions;

namespace MessageBroker.Events.UserEvents
{
    public record GetListUserDataByListRoleEvent : BaseEvent
    {
        public List<string> Roles { get; init; } = [];
    }
}