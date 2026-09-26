using MessageBroker.Abstractions;

namespace MessageBroker.Events.UserEvents
{
    public record UpdateUserEvent : BaseEvent
    {
        public int Id { get; init; }
        public string? Name { get; init; }
        public string? DateOfBirth { get; init; }
        public string? Gender { get; init; }
        public string? Address { get; init; }
        public string? Image { get; init; }
    }
}
