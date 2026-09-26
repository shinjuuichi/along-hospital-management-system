using MessageBroker.Abstractions;

namespace MessageBroker.Events.MedicalHistoryEvents
{
    public record CreateMedicalHistoryFromAppointmentEvent : BaseEvent
    {
        public int PatientId { get; init; }

        public int SpecialtyId { get; init; }
    }
}