using MessageBroker.Abstractions;

namespace MessageBroker.Events.MedicalOrderEvents
{
    public record GetListMedicalOrderDataByMedicalHistoryIdEvent : BaseEvent
    {
        public int MedicalHistoryId { get; init; }
    }
}
