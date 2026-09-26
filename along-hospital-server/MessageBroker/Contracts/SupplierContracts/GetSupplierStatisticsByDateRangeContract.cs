using MessageBroker.Abstractions;
using MessageBroker.Contracts.StatisticsContracts;

namespace MessageBroker.Contracts.SupplierContracts
{
    public record GetSupplierStatisticsByDateRangeContract : BaseContract
    {
        public int TotalSuppliers { get; init; }
        public StatisticsChartContract SuppliersOverTime { get; init; } = new();
    }
}
