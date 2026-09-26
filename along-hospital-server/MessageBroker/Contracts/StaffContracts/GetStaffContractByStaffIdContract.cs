using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.StaffContracts
{
    public record GetStaffContractByStaffIdContract : BaseContract
    {
        public double HourlyRate { get; init; }

        public double InsuranceSalaryRate { get; init; }

        public int DependentQuantity { get; init; }

        public int RegionalWageCode { get; init; }

        public double RegionalWageMonthlyWage { get; init; }
    }
}
