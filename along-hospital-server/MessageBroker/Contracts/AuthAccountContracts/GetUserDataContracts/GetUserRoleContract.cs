using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts
{
    public record GetUserRoleByUserIdContract : BaseContract
    {
        public string Role { get; set; } = string.Empty;
    }
}
