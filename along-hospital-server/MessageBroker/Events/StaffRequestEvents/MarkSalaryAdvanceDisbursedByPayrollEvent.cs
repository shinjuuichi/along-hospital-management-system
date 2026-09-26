using MessageBroker.Abstractions;

namespace MessageBroker.Events.StaffRequestEvents
{
    public record MarkSalaryAdvanceDisbursedByPayrollEvent : BaseEvent
    {
        public int StaffId { get; init; }

        public int PayrollId { get; init; }
    }
}
