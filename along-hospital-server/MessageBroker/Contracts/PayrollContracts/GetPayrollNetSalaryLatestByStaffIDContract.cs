using MessageBroker.Abstractions;

namespace MessageBroker.Contracts.PayrollContracts
{
    public record GetPayrollNetSalaryLatestByStaffIDContract : BaseContract
    {
        public double NetSalary { get; init; }
    }
}