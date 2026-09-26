using MessageBroker.Abstractions;

namespace MessageBroker.Events.AttendanceEvents
{
    public record GetAttendanceByStaffsAndRangeEvent : BaseEvent
    {
        public List<int> StaffIds { get; init; } = [];

        public DateTime FromDate { get; init; }

        public DateTime ToDate { get; init; }
    }
}