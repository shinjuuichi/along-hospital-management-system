using MessageBroker.Abstractions;

namespace MessageBroker.Events.UserEvents;

public record GetUserEmailsByRolesEvent : BaseEvent
{
    public List<string> Roles { get; init; } = [];
    public List<int> ExcludeUserIds { get; init; } = [];
}
