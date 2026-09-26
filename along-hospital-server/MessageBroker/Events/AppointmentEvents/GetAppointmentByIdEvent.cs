using MessageBroker.Abstractions;

namespace MessageBroker.Events.AppointmentEvents
{
    public record GetAppointmentByIdEvent : BaseEvent
    {
        public int Id { get; init; }
    }

    public record GetListAppointmentDataByIdsEvent : BaseEvent
    {
        public List<int> Ids { get; init; } = [];
    }
}
