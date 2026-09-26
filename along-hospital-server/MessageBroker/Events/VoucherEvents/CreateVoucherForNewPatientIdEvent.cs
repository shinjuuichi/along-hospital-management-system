using MessageBroker.Abstractions;

namespace MessageBroker.Events.VoucherEvents
{
    public record CreateVoucherForNewPatientIdEvent : BaseEvent
    {
        public int UserId { get; init; }
    }
}
