using MessageBroker.Abstractions;

namespace MessageBroker.Events.InventoryEvents
{
    public record CheckQuantityOfListSKUDataEvent : BaseEvent
    {
        public List<CheckQuantityOfSKUEventItem> SKUEventItems { get; init; } = [];
    }

    public record CheckQuantityOfSKUEventItem
    {
        public string? SKUCode { get; init; }
        public int Quantity { get; init; }
    }
}