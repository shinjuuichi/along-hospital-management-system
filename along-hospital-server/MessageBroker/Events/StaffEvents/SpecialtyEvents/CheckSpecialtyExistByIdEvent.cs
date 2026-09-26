using MessageBroker.Abstractions;

namespace MessageBroker.Events.StaffEvents.SpecialtyEvents
{
    public record CheckSpecialtyExistByIdEvent : BaseEvent
    {
        public int SpecialtyId { get; init; }
    }

    public record CheckSpecialtyExistByIdContract : BaseContract;
}