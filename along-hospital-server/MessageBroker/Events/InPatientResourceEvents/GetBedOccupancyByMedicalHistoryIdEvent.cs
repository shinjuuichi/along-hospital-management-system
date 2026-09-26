using MessageBroker.Abstractions;

namespace MessageBroker.Events.InPatientResourceEvents
{
    public record GetBedOccupancyByMedicalHistoryIdEvent : BaseEvent
    {
        public int MedicalHistoryId { get; init; }
    }

    public record GetListBedOccupancyDataByMedicalHistoryIdEvent : BaseEvent
    {
        public int MedicalHistoryId { get; init; }
    }
}
