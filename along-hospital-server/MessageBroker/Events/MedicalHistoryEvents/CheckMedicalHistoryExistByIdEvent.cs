using MessageBroker.Abstractions;

namespace MessageBroker.Events.MedicalHistoryEvents
{
    public record CheckMedicalHistoryExistByIdEvent : BaseEvent
    {
        public int Id { get; init; }
    }

    public record CheckMedicalHistoryExistByIdContract : BaseContract;
}