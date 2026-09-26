using MessageBroker.Abstractions;

namespace MessageBroker.Events.MedicalHistoryEvents
{
    public record GetListMedicalHistoryOccupancySummaryByIdsEvent : BaseEvent
    {
        public List<int> Ids { get; init; } = [];
    }
}
