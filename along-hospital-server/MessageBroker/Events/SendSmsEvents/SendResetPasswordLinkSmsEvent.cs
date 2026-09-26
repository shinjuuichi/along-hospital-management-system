using MessageBroker.Abstractions;

namespace MessageBroker.Events.SendSmsEvents
{
    public record SendResetPasswordLinkSmsEvent : BaseEvent
    {
        public string PhoneNumber { get; init; } = default!;
        public string Link { get; init; } = default!;
    }
}
