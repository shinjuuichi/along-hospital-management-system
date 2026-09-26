using MessageBroker.Abstractions;

namespace MessageBroker.Events.MedicalOrderEvents
{
    public record CancelPendingOrDraftMedicalOrdersByMedicalHistoryIdEvent : BaseEvent
    {
        public int MedicalHistoryId { get; init; }
    }

    public record CancelPendingOrDraftMedicalOrdersByMedicalHistoryIdContract : BaseContract;
}