using MessageBroker.Abstractions;

namespace MessageBroker.Events.SendSmsEvents
{
    public record SendVerificationLinkSmsEvent : BaseEvent
    {
        public string PhoneNumber { get; init; } = default!;
        public string Link { get; init; } = default!;
    }
}
