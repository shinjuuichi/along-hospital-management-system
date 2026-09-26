using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.UserContracts;

public record UserEmailData
{
    public int UserId { get; init; }
    public string? Name { get; init; }
    public string? Email { get; init; }
}

public record GetUserEmailsByRolesContract : BaseContract
{
    public List<UserEmailData> Data { get; init; } = [];
}
