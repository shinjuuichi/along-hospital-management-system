using MessageBroker.Abstractions;

namespace MessageBroker.Events.StaffRequestEvents
{
    public record GetStaffRequestStatisticsByDateRangeEvent : BaseEvent
    {
        public DateOnly FromDate { get; init; }
        public DateOnly ToDate { get; init; }
    }
}
