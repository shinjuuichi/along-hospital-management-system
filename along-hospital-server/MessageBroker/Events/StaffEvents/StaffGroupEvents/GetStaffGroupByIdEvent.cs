using MessageBroker.Abstractions;

namespace MessageBroker.Events.StaffEvents.StaffGroupEvents
{
    public record GetStaffGroupByIdEvent : BaseEvent
    {
        public int Id { get; init; }
    }

    public record GetListStaffGroupDataByIdsEvent : BaseEvent
    {
        public List<int> Ids { get; init; } = [];
    }
}