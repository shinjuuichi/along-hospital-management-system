using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.StaffRequestContracts
{
    public record GetUndisbursedSalaryAdvanceByStaffIdContract : BaseContract
    {
        public int Id { get; init; }

        public double Amount { get; init; }

        public string? Reason { get; init; }

        public int CreatedBy { get; init; }

        public DateTime CreationDate { get; init; }

        public DateTime? ModificationDate { get; init; }

        public int? ModifiedBy { get; init; }
    }
}
