using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.UserContracts
{
    public record GetUserDataByRoleContract : BaseContract
    {
        public string? Name { get; init; }

        public string? Image { get; init; }

        public DateOnly DateOfBirth { get; init; }

        public string? Gender { get; init; }

        public string? Address { get; init; }

        public string? Role { get; init; }
    }

    public record GetListUserDataByRoleContract : BaseContract
    {
        public List<GetUserDataByRoleContract> Data { get; init; } = [];
    }
}