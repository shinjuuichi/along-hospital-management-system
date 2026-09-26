using MessageBroker.Abstractions;

namespace MessageBroker.Events.PayrollEvents
{
    public record GetPayrollNetSalaryLatestByStaffIdEvent : BaseEvent
    {
        public int StaffId { get; init; }
    }
}