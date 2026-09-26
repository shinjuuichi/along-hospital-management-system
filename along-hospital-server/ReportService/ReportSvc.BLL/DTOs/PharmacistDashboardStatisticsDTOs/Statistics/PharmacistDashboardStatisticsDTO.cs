namespace ReportSvc.BLL.DTOs.PharmacistDashboardStatisticsDTOs.Statistics
{
    public class PharmacistDashboardStatisticsDTO
    {
        public PharmacistDashboardOverviewDTO? Overview { get; set; }
        public PharmacistOrderStatisticsDTO? Order { get; set; }
        public PharmacistTopMedicineStatisticsDTO? TopMedicines { get; set; }
    }
}
