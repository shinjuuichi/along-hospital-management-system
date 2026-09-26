using MessageBroker.Abstractions;

namespace MessageBroker.Events.WorkScheduleEvents
{
    public record ValidateStaffActionEvent : BaseEvent
    {
        public DateTime ActionDateTime { get; init; } = DateTime.UtcNow;

        public int StaffId { get; init; }

        public int RoomId { get; init; }
    }

    public record ValidateStaffActionContract : BaseContract;
}