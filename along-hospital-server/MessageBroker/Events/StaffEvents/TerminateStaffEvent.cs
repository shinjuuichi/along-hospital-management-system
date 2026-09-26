using MessageBroker.Abstractions;

namespace MessageBroker.Events.StaffEvents
{
    public record TerminateStaffEvent : BaseEvent
    {
        public List<int> StaffIds { get; init; } = [];
    }
}
