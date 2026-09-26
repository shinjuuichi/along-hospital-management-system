using MessageBroker.Contracts.StatisticsContracts;

namespace InventorySvc.BLL.DTOs.StatisticsDTOs
{
    public class InventoryStatisticsDTO
    {
        public int TotalInventoryItems { get; set; }
        public int LowStockItems { get; set; }
        public int OutOfStockItems { get; set; }
        public int TotalQuantity { get; set; }
        public StatisticsDistributionContract StockStatus { get; set; } = new();
        public StatisticsChartContract InventoryOverTime { get; set; } = new();
    }
}
