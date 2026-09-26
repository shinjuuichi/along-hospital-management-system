namespace MessageBroker.Events.AuthAccountEvents.UpdateAccountEvents
{
    public record UpdateStaffToUserToAuthEvent : UpdateUserToAuthEvent
    {
        public string? SignatureImage { get; init; }
    }
}