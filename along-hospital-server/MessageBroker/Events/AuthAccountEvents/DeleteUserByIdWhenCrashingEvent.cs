using MessageBroker.Abstractions;

namespace MessageBroker.Events.AuthAccountEvents
{
    public record DeleteUserByIdWhenCrashingEvent : BaseEvent
    {
        public int Id { get; init; }
    }
}
