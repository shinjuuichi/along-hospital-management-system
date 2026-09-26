using MessageBroker.Abstractions;

namespace MessageBroker.Events.InventoryEvents
{
    public record AddQuantityFromCancelOrderEvent : BaseEvent
    {
        public List<AddQuantityFromCancelOrderEventItem> Items { get; init; } = [];
    }

    public record AddQuantityFromCancelOrderEventItem
    {
        public string? SKUCode { get; init; }
        public int Quantity { get; init; }
    }
}
