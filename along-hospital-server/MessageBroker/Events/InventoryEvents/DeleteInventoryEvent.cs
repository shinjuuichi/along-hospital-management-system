using MessageBroker.Abstractions;

namespace MessageBroker.Events.InventoryEvents
{
    public record DeleteInventoryEvent : BaseEvent
    {
        public string? SKUCode { get; init; }
    }
}