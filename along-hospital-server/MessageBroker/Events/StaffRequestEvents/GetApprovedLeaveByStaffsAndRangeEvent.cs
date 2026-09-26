using MessageBroker.Abstractions;

namespace MessageBroker.Events.StaffRequestEvents
{
    public record GetApprovedLeaveByStaffsAndRangeEvent : BaseEvent
    {
        public List<int> StaffIds { get; init; } = [];

        public DateOnly FromDate { get; init; }

        public DateOnly ToDate { get; init; }
    }
}