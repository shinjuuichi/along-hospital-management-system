using MessageBroker.Abstractions;
using MessageBroker.Contracts.StatisticsContracts;

namespace MessageBroker.Contracts.InventoryContracts
{
    public record GetInventoryStatisticsByDateRangeContract : BaseContract
    {
        public int TotalInventoryItems { get; init; }
        public int LowStockItems { get; init; }
        public int OutOfStockItems { get; init; }
        public int TotalQuantity { get; init; }
        public StatisticsDistributionContract StockStatus { get; init; } = new();
        public StatisticsChartContract InventoryOverTime { get; init; } = new();
    }
}
