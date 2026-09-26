using MessageBroker.Abstractions;
using MessageBroker.Contracts.StatisticsContracts;

namespace MessageBroker.Contracts.OrderContracts
{
    public record GetOrderStatisticsByDateRangeContract : BaseContract
    {
        public double Revenue { get; init; }
        public int Orders { get; init; }
        public int PendingOrders { get; init; }
        public int CompletedOrders { get; init; }
        public StatisticsChartContract RevenueOverTime { get; init; } = new();
        public StatisticsChartContract OrdersOverTime { get; init; } = new();
        public StatisticsDistributionContract OrderStatus { get; init; } = new();
    }
}
