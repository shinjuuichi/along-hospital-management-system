using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.StaffContracts
{
    public record GetStaffProfileContract : BaseContract
    {
        public string? Status { get; init; }

        public string? SpecialtyName { get; init; }

        public string? QualificationName { get; init; }

        public string? BankCode { get; init; }

        public string? AccountNumber { get; init; }

        public int DependentQuantity { get; init; }
    }
}
