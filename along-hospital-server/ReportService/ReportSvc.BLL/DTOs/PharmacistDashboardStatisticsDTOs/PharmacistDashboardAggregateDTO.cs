using ReportSvc.BLL.DTOs.PharmacistDashboardStatisticsDTOs.Statistics;

namespace ReportSvc.BLL.DTOs.PharmacistDashboardStatisticsDTOs
{
    internal class PharmacistDashboardAggregateDTO
    {
        public PharmacistOrderStatisticsDTO? Order { get; set; }
        public PharmacistTopMedicineStatisticsDTO? TopMedicines { get; set; }
    }
}
