using MessageBroker.Abstractions;

namespace MessageBroker.Events.SendEmailEvents
{
    public record SendVerificationLinkEmailEvent : BaseEvent
    {
        public string Email { get; init; } = default!;
        public string Subject { get; init; } = default!;
        public string Link { get; init; } = default!;
    }
}
