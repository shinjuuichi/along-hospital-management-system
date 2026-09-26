using MessageBroker.Contracts.StatisticsContracts;

namespace SupplierSvc.BLL.DTOs.StatisticsDTOs
{
    public class ImportStatisticsDTO
    {
        public int TotalImports { get; set; }
        public double TotalImportValue { get; set; }
        public StatisticsChartContract? ImportsOverTime { get; set; }
        public StatisticsDistributionContract? ImportBySupplier { get; set; }
    }
}