using MessageBroker.Abstractions;

namespace MessageBroker.Events.StaffEvents
{
    public record ChangeAuthStatusEvent : BaseEvent
    {
        public int UserId { get; init; }
        public string? Status { get; init; }
    }
}