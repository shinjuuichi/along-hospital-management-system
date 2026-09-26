using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.UserContracts
{
    public record UpdateUserContract : BaseContract
    {
        public int Id { get; init; }
        public string? Name { get; init; }
        public string? Role { get; init; }
        public string? Image { get; init; }
        public DateTime DateOfBirth { get; init; }
        public string? Gender { get; init; }
        public string? Address { get; init; }
    }
}
