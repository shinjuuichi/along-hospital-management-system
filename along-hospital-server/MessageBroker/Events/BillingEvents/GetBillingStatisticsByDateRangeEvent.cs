using MessageBroker.Abstractions;

namespace MessageBroker.Events.BillingEvents
{
    public record GetBillingStatisticsByDateRangeEvent : BaseEvent
    {
        public DateOnly FromDate { get; init; }
        public DateOnly ToDate { get; init; }
    }
}
