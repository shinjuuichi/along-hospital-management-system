using MessageBroker.Abstractions;

namespace MessageBroker.Events.StaffRequestEvents
{
    public record GetUndisbursedSalaryAdvanceByStaffIdEvent : BaseEvent
    {
        public int StaffId { get; init; }
    }
}