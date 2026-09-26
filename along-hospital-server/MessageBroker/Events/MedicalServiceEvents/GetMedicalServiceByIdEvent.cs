using MessageBroker.Abstractions;

namespace MessageBroker.Events.MedicalServiceEvents
{
    public record GetMedicalServiceByIdEvent : BaseEvent
    {
        public int Id { get; init; }
    }

    public record GetListMedicalServiceDataByIdsEvent : BaseEvent
    {
        public List<int> Ids { get; init; } = [];
    }
}