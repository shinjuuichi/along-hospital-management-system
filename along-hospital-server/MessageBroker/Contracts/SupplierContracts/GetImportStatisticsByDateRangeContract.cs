using MessageBroker.Abstractions;
using MessageBroker.Contracts.StatisticsContracts;

namespace MessageBroker.Contracts.SupplierContracts
{
    public record GetImportStatisticsByDateRangeContract : BaseContract
    {
        public int TotalImports { get; init; }
        public double TotalImportValue { get; init; }
        public StatisticsChartContract ImportsOverTime { get; init; } = new();
        public StatisticsDistributionContract ImportBySupplier { get; init; } = new();
    }
}
