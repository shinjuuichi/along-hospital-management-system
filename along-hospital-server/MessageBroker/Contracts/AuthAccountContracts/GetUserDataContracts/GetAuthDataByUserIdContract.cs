using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts
{
    public record GetAuthDataByUserIdContract : BaseContract
    {
        public int UserId { get; init; }

        public string? Phone { get; init; }

        public string? Email { get; init; }
    }

    public record GetListAuthDataByUserIdsContract : BaseContract
    {
        public List<GetAuthDataByUserIdContract> Data { get; init; } = [];
    }
}