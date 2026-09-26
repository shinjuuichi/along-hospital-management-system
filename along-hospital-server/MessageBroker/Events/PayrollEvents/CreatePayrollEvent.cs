using MessageBroker.Abstractions;

namespace MessageBroker.Events.PayrollEvents
{
    public record CreatePayrollEvent : BaseEvent
    {
        public int TotalWorkedMinutes { get; init; }

        public int OvertimeMinutes { get; init; }

        public int LateMinutes { get; init; }

        public int EarlyLeaveMinutes { get; init; }

        public int StaffId { get; init; }
    }
}