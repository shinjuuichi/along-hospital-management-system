using MessageBroker.Abstractions;

namespace MessageBroker.Events.InPatientResourceEvents
{
    public record DischargeBedOccupancyByMedicalHistoryIdEvent : BaseEvent
    {
        public int MedicalHistoryId { get; init; }
    }

    public record DischargeBedOccupancyByMedicalHistoryIdContract : BaseContract;
}
