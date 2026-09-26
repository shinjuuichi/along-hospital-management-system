using MessageBroker.Abstractions;
using MessageBroker.Contracts.StatisticsContracts;

namespace MessageBroker.Contracts.BillingContracts
{
    public record GetBillingStatisticsByDateRangeContract : BaseContract
    {
        public int TotalInvoices { get; init; }
        public int CompletedInvoices { get; init; }
        public int PendingInvoices { get; init; }
        public int CancelledInvoices { get; init; }
        public double GrossInvoiceAmount { get; init; }
        public double CollectedAmount { get; init; }
        public double RefundAmount { get; init; }
        public double NetCollectedAmount { get; init; }
        public int PendingRefunds { get; init; }
        public int ApprovedRefunds { get; init; }
        public int CancelledRefunds { get; init; }
        public StatisticsDistributionContract InvoiceStatus { get; init; } = new();
        public StatisticsDistributionContract RefundStatus { get; init; } = new();
        public StatisticsChartContract InvoicesOverTime { get; init; } = new();
        public StatisticsChartContract CollectionsOverTime { get; init; } = new();
        public StatisticsChartContract RefundsOverTime { get; init; } = new();
    }
}
