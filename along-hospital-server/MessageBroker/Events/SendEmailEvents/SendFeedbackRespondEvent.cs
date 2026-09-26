using MessageBroker.Abstractions;

namespace MessageBroker.Events.SendEmailEvents
{
    public record SendFeedbackRespondEvent : BaseEvent
    {
        public string? Email { get; init; }
        
        public string? Content { get; init; }

        public string? StaffName { get; init; }
    }
}
