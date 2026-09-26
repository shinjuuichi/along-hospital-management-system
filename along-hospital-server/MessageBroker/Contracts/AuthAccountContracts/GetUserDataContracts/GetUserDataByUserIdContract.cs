using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts
{
    public record GetUserDataByUserIdContract : GetAuthDataByUserIdContract
    {
        public string? Name { get; init; }

        public string? Image { get; init; }

        public DateOnly DateOfBirth { get; init; }

        public string? Gender { get; init; }

        public string? Address { get; init; }

        public string? Role { get; init; }
    }

    public record GetListUserDataByUserIdsContract : BaseContract
    {
        public List<GetUserDataByUserIdContract> Data { get; init; } = [];
    }
}