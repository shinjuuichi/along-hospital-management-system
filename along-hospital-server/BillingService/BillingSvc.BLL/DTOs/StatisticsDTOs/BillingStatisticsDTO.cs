using MessageBroker.Contracts.StatisticsContracts;

namespace BillingSvc.BLL.DTOs.StatisticsDTOs
{
    public class BillingStatisticsDTO
    {
        public int TotalInvoices { get; set; }
        public int CompletedInvoices { get; set; }
        public int PendingInvoices { get; set; }
        public int CancelledInvoices { get; set; }
        public double GrossInvoiceAmount { get; set; }
        public double CollectedAmount { get; set; }
        public double RefundAmount { get; set; }
        public double NetCollectedAmount { get; set; }
        public int PendingRefunds { get; set; }
        public int ApprovedRefunds { get; set; }
        public int CancelledRefunds { get; set; }
        public StatisticsDistributionContract InvoiceStatus { get; set; } = new();
        public StatisticsDistributionContract RefundStatus { get; set; } = new();
        public StatisticsChartContract InvoicesOverTime { get; set; } = new();
        public StatisticsChartContract CollectionsOverTime { get; set; } = new();
        public StatisticsChartContract RefundsOverTime { get; set; } = new();
    }
}
