using MessageBroker.Abstractions;

namespace MessageBroker.Events.PatientEvents
{
    public record CreatePatientEvent : BaseEvent
    {
        public int Id { get; init; }
        public int UserId { get; init; }
    }

    public record CreatePatientContract : BaseContract;
}
