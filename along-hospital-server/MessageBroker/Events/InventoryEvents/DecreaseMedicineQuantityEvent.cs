using MessageBroker.Abstractions;

namespace MessageBroker.Events.InventoryEvents
{
    public record DecreaseInventoryQuantityEvent : BaseEvent
    {
        public List<DecreaseInventoryQuantityEventItem> DecreaseInventoryQuantityEventItems { get; init; } = [];
    }

    public record DecreaseInventoryQuantityEventItem
    {
        public string? SKUCode { get; init; }
        public int Quantity { get; init; }
    }
}