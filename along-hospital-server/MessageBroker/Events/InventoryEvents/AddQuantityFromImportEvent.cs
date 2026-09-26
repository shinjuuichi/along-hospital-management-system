using MessageBroker.Abstractions;

namespace MessageBroker.Events.InventoryEvents
{
    public record AddQuantityFromImportEvent : BaseEvent
    {
        public string? SKUCode { get; init; }
        public int Quantity { get; init; }
        public DateTime? LastImportDate { get; init; }
    }
}