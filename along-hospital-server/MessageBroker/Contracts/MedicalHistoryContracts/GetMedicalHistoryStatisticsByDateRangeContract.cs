using MessageBroker.Abstractions;
using MessageBroker.Contracts.StatisticsContracts;

namespace MessageBroker.Contracts.MedicalHistoryContracts
{
    public record GetMedicalHistoryStatisticsByDateRangeContract : BaseContract
    {
        public int TotalMedicalHistories { get; init; }
        public int InpatientHistories { get; init; }
        public int OutpatientHistories { get; init; }
        public int PendingPaymentHistories { get; init; }
        public int CompletedHistories { get; init; }
        public StatisticsDistributionContract MedicalHistoryStatus { get; init; } = new();
        public StatisticsDistributionContract MedicalHistoryType { get; init; } = new();
        public StatisticsChartContract AdmissionsOverTime { get; init; } = new();
    }
}
