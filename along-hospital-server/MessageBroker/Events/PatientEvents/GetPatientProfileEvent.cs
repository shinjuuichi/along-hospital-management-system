using MessageBroker.Abstractions;

namespace MessageBroker.Events.PatientEvents
{
    public record GetPatientProfileEvent : BaseEvent
    {
        public int PatientId { get; init; }
    }
}
