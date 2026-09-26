using MessageBroker.Abstractions;

namespace MessageBroker.Events.InPatientResourceEvents
{
    public record GetRoomByIdEvent : BaseEvent
    {
        public int Id { get; init; }
    }

    public record GetListRoomDataByIdsEvent : BaseEvent
    {
        public List<int> Ids { get; init; } = [];
    }
}