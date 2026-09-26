using MessageBroker.Abstractions;

namespace MessageBroker.Events.FeedbackEvents
{
    public record PredictFeedbackToxicEvent : BaseEvent
    {
        public int Id { get; init; }

        public string? Content { get; init; }
    }
}