using MessageBroker.Abstractions;

namespace MessageBroker.Events.MedicalHistoryEvents
{
    public record AssignDoctorToMedicalHistoryEvent : BaseEvent
    {
        public int MedicalHistoryId { get; init; }

        public int DoctorId { get; init; }
    }

    public record AssignDoctorToMedicalHistoryContract : BaseContract;
}