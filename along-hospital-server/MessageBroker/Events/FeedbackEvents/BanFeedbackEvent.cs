using MessageBroker.Abstractions;

namespace MessageBroker.Events.FeedbackEvents
{
    public record BanFeedbackEvent : BaseEvent
    {
        public int UserId { get; init; }
    }
}