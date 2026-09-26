using MessageBroker.Abstractions;

namespace MessageBroker.Events.PatientEvents
{
    public record CheckPatientExistByIdEvent : BaseEvent
    {
        public int PatientId { get; init; }
    }
}