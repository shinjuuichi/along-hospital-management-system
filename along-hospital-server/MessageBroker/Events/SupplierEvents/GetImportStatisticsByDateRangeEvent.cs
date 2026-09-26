using MessageBroker.Abstractions;

namespace MessageBroker.Events.SupplierEvents
{
    public record GetImportStatisticsByDateRangeEvent : BaseEvent
    {
        public DateOnly FromDate { get; init; }
        public DateOnly ToDate { get; init; }
    }
}
