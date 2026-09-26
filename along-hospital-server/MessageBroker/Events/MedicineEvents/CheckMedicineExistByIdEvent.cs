using MessageBroker.Abstractions;

namespace MessageBroker.Events.MedicineEvents
{
    public record CheckMedicineExistByIdEvent : BaseEvent
    {
        public int MedicineId { get; init; }
    }

    public record CheckMedicineExistByIdContract : BaseContract;
}
