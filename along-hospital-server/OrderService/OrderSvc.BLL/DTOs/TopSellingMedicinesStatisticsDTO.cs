namespace OrderSvc.BLL.DTOs
{
    public class TopSellingMedicinesStatisticsDTO
    {
        public int TopN { get; init; }
        public List<TopSellingMedicineItemDTO> Items { get; init; } = [];
    }

    public class TopSellingMedicineItemDTO
    {
        public string SKUCode { get; init; } = string.Empty;
        public string MedicineName { get; init; } = string.Empty;
        public int QuantitySold { get; init; }
    }
}
