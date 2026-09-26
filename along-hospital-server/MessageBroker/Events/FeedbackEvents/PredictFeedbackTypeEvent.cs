using MessageBroker.Abstractions;

namespace MessageBroker.Events.FeedbackEvents
{
    public record PredictFeedbackTypeEvent : BaseEvent
    {
        public int Id { get; init; }

        public string? Content { get; init; }
    }
}