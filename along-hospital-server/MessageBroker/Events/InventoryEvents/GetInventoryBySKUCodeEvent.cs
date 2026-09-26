using MessageBroker.Abstractions;

namespace MessageBroker.Events.InventoryEvents
{
    public record GetInventoryBySKUCodeEvent : BaseEvent
    {
        public string? SKUCode { get; init; }
    }

    public record GetListInventoryDataBySKUCodesEvent : BaseEvent
    {
        public List<string> SKUCodes { get; init; } = [];
    }
}