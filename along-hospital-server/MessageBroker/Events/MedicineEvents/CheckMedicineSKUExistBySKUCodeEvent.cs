using MessageBroker.Abstractions;

namespace MessageBroker.Events.MedicineEvents
{
    public record CheckMedicineSKUExistBySKUCodeEvent : BaseEvent
    {
        public string? SKUCode { get; init; }
    }

    public record CheckMedicineSKUExistBySKUCodeContract : BaseContract;
}
