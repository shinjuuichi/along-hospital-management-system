using MessageBroker.Abstractions;

namespace MessageBroker.Events.MedicineEvents
{
    public record CheckMedicineSKUExistByIdEvent : BaseEvent
    {
        public int MedicineSKUId { get; init; }
    }

    public record CheckMedicineSKUExistByIdContract : BaseContract;
}
