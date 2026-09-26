using MessageBroker.Abstractions;

namespace MessageBroker.Events.MedicalHistoryEvents
{
    public record GetMedicalHistoryByIdEvent : BaseEvent
    {
        public int Id { get; init; }
    }

    public record GetListMedicalHistoryDataByIdsEvent : BaseEvent
    {
        public List<int> Ids { get; init; } = [];
    }
}