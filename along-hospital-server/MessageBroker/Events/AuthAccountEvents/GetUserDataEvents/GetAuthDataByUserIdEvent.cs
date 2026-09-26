using MessageBroker.Abstractions;

namespace MessageBroker.Events.AuthAccountEvents.GetUserDataEvents
{
    public record GetAuthDataByUserIdEvent : BaseEvent
    {
        public int UserId { get; init; }
    }

    public record GetListAuthDataByUserIdsEvent : BaseEvent
    {
        public List<int> UserIds { get; init; } = [];
    }
}