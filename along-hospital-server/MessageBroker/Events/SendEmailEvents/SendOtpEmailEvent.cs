using MessageBroker.Abstractions;

namespace MessageBroker.Events.SendEmailEvents
{
    public record SendOtpEmailEvent : BaseEvent
    {
        public string Email { get; init; } = default!;

        public string Subject { get; init; } = default!;

        public string Otp { get; init; } = default!;
    }
}