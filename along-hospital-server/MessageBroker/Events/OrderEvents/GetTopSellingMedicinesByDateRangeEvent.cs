using MessageBroker.Abstractions;

namespace MessageBroker.Events.OrderEvents
{
    public record GetTopSellingMedicinesByDateRangeEvent : BaseEvent
    {
        public DateOnly FromDate { get; init; }
        public DateOnly ToDate { get; init; }
        public int TopN { get; init; } = 5;
    }
}
