using MessageBroker.Abstractions;

namespace MessageBroker.Events.InventoryEvents
{
    public record CreateInventoryEvent : BaseEvent
    {
        public int Quantity { get; init; }
        public DateTime? LastImportDate { get; init; }
        public int? MinQuantity { get; init; }
        public int? MaxQuantity { get; init; }
        public string? SKUCode { get; init; }
    }
}