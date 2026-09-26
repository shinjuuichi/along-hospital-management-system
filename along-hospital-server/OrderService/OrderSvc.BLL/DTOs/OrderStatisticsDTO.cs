namespace OrderSvc.BLL.DTOs
{
    public class OrderStatisticsDTO
    {
        public double Revenue { get; init; }
        public int Orders { get; init; }
        public int PendingOrders { get; init; }
        public int CompletedOrders { get; init; }
        public StatisticsChartDTO RevenueOverTime { get; init; } = new();
        public StatisticsChartDTO OrdersOverTime { get; init; } = new();
        public StatisticsDistributionDTO OrderStatus { get; init; } = new();
    }

    public class StatisticsChartDTO
    {
        public List<string> Labels { get; init; } = [];
        public List<StatisticsChartDatasetDTO> Datasets { get; init; } = [];
    }

    public class StatisticsChartDatasetDTO
    {
        public string Label { get; init; } = string.Empty;
        public List<double> Data { get; init; } = [];
    }

    public class StatisticsDistributionDTO
    {
        public List<string> Labels { get; init; } = [];
        public List<int> Data { get; init; } = [];
    }
}
