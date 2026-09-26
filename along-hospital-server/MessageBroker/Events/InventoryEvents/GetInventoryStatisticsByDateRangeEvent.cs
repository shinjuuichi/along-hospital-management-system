using MessageBroker.Abstractions;

namespace MessageBroker.Events.InventoryEvents
{
    public record GetInventoryStatisticsByDateRangeEvent : BaseEvent
    {
        public DateOnly FromDate { get; init; }
        public DateOnly ToDate { get; init; }
    }
}
