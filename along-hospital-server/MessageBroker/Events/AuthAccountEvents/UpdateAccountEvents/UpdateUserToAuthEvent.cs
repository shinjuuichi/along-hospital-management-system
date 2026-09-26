namespace MessageBroker.Events.AuthAccountEvents.UpdateAccountEvents
{
    public record UpdateUserToAuthEvent : UpdateAuthEvent
    {
        public string? Role { get; init; }

        public string? Name { get; init; }

        public string? Gender { get; init; }

        public string? Address { get; init; }

        public string? Image { get; init; }

        public DateOnly? DateOfBirth { get; init; }
    }
}