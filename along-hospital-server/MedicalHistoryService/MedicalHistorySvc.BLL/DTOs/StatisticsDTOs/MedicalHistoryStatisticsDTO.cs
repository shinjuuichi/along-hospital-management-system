using MessageBroker.Contracts.StatisticsContracts;

namespace MedicalHistorySvc.BLL.DTOs.StatisticsDTOs
{
    public class MedicalHistoryStatisticsDTO
    {
        public int TotalMedicalHistories { get; set; }
        public int InpatientHistories { get; set; }
        public int OutpatientHistories { get; set; }
        public int PendingPaymentHistories { get; set; }
        public int CompletedHistories { get; set; }
        public StatisticsDistributionContract MedicalHistoryStatus { get; set; } = new();
        public StatisticsDistributionContract MedicalHistoryType { get; set; } = new();
        public StatisticsChartContract AdmissionsOverTime { get; set; } = new();
    }
}
