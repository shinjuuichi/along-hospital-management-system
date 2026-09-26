using MessageBroker.Abstractions;

namespace MessageBroker.Events.MedicineEvents
{
    public record GetMedicineSKUBySKUCodeEvent : BaseEvent
    {
        public string? SKUCode { get; init; }
    }

    public record GetListMedicineSKUDataBySKUCodesEvent : BaseEvent
    {
        public List<string> SKUCodes { get; init; } = [];
    }

    public record GetPublicMedicineSKUsBySKUCodesEvent : BaseEvent
    {
        public List<string> SKUCodes { get; init; } = [];
    }
}