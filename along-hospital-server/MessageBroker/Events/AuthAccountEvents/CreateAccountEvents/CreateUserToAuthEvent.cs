using MessageBroker.Abstractions;

namespace MessageBroker.Events.AuthAccountEvents.CreateAccountEvents
{
    public record CreateUserToAuthEvent : BaseEvent
    {
        // Auth
        public string? Email { get; init; }

        public string? Phone { get; init; }

        public int UserId { get; init; }

        // User
        public string? Name { get; init; }

        public string? Image { get; init; }

        public string? Role { get; init; }

        public DateOnly DateOfBirth { get; init; }

        public string? Gender { get; init; }

        public string? Address { get; init; }
    }
}