using MessageBroker.Abstractions;

namespace MessageBroker.Events.WorkScheduleEvents
{
    public record GetListShiftsDataByIdsEvent : BaseEvent
    {
        public List<int> Ids { get; init; } = [];
    }
}
