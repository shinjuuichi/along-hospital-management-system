using MessageBroker.Abstractions;

namespace MessageBroker.Events.SendSmsEvents
{
    public record SendOtpSmsEvent : BaseEvent
    {
        public string PhoneNumber { get; init; } = default!;

        public string Otp { get; init; } = default!;
    }
}
