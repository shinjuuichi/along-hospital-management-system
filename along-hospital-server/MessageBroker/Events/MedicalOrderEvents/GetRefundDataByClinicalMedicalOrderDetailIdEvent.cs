using MessageBroker.Abstractions;

namespace MessageBroker.Events.MedicalOrderEvents
{
    public record GetRefundDataByClinicalMedicalOrderDetailIdEvent : BaseEvent
    {
        public string? ClinicalMedicalOrderDetailId { get; init; }
    }
}
