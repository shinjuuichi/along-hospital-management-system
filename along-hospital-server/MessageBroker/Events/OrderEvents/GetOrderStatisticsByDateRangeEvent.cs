using MessageBroker.Abstractions;

namespace MessageBroker.Events.OrderEvents
{
    public record GetOrderStatisticsByDateRangeEvent : BaseEvent
    {
        public DateOnly FromDate { get; init; }
        public DateOnly ToDate { get; init; }
    }
}

