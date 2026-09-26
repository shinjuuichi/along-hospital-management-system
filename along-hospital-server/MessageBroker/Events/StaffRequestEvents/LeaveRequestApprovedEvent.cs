using MessageBroker.Abstractions;

namespace MessageBroker.Events.StaffRequestEvents
{
    public record LeaveRequestApprovedEvent : BaseEvent
    {
        public int StaffId { get; init; }

        public DateOnly FromDate { get; init; }

        public DateOnly ToDate { get; init; }

        public string? LeaveUnit { get; init; }

        public int? ShiftId { get; init; }
    }
}