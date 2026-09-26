using MessageBroker.Abstractions;

namespace MessageBroker.Events.MedicalHistoryEvents
{
    public record DraftMedicalHistoryWhenGeneralPaymentCompletedEvent : BaseEvent
    {
        public int Id { get; init; }
    }
}
