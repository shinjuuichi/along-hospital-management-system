using MessageBroker.Abstractions;
using MessageBroker.Contracts.BillingContracts;

namespace MessageBroker.Events.BillingEvents
{
    public record CreateRefundWhenClinicalMedicalOrderFailedEvent : BaseEvent
    {
        public CreateRefundContract? Data { get; init; }
    }
}
