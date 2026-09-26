namespace ReportSvc.BLL.DTOs.PharmacistDashboardStatisticsDTOs.Statistics
{
    public class PharmacistTopMedicineStatisticsDTO
    {
        public int TopN { get; set; }
        public List<PharmacistTopMedicineItemDTO> Items { get; set; } = [];
    }

    public class PharmacistTopMedicineItemDTO
    {
        public string SKUCode { get; set; } = string.Empty;
        public string MedicineName { get; set; } = string.Empty;
        public int QuantitySold { get; set; }
    }
}
