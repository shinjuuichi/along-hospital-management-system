using MessageBroker.Abstractions;

namespace MessageBroker.Events.MedicineEvents
{
    public record GetMedicineSKUByIdEvent : BaseEvent
    {
        public int Id { get; init; }
    }

    public record GetListMedicineSKUDataByIdsEvent : BaseEvent
    {
        public List<int> Ids { get; init; } = [];
    }
}
