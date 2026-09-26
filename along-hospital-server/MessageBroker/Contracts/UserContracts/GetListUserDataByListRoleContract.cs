using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.UserContracts
{
    public record GetUserDataByListRoleContract : BaseContract
    {
        public int UserId { get; init; }

        public string? Name { get; init; }

        public string? Image { get; init; }

        public DateOnly DateOfBirth { get; init; }

        public string? Gender { get; init; }

        public string? Address { get; init; }

        public string? Role { get; init; }
    }

    public record GetListUserDataByListRoleContract : BaseContract
    {
        public List<GetUserDataByListRoleContract> Data { get; init; } = [];
    }
}
