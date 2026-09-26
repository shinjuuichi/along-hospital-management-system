namespace VoucherSvc.BLL.DTOs.DiscountDTOs.PreviewListMedicineDiscountDTOs
{
    public class PreviewListMedicineDiscountRequestDTO
    {
        public List<PreviewMedicineItemDTO> Medicines { get; set; } = [];
    }

    public class PreviewMedicineItemDTO
    {
        public int MedicineId { get; set; }

        public string? SKUCode { get; set; }

        public double Price { get; set; }
    };
}
