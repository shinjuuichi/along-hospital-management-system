using MessageBroker.Abstractions;

namespace MessageBroker.Events.MedicalHistoryEvents
{
    public record PredictComplaintTypeEvent : BaseEvent
    {
        public int ComplaintId { get; init; }
    }
}