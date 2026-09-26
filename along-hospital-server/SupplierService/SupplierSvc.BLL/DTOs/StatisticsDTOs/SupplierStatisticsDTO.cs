using MessageBroker.Contracts.StatisticsContracts;

namespace SupplierSvc.BLL.DTOs.StatisticsDTOs
{
    public class SupplierStatisticsDTO
    {
        public int TotalSuppliers { get; set; }
        public StatisticsChartContract? SuppliersOverTime { get; set; }
    }
}