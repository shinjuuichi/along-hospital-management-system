using MessageBroker.Abstractions;

namespace MessageBroker.Events.UserEvents
{
    public record GetUserGrowthStatisticsByDateRangeEvent : BaseEvent
    {
        public DateOnly FromDate { get; init; }
        public DateOnly ToDate { get; init; }
    }
}
