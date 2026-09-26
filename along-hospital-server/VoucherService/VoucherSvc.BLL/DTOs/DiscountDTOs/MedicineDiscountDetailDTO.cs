namespace VoucherSvc.BLL.DTOs.DiscountDTOs
{
    public class MedicineDiscountDetailDTO
    {
        public int MedicineId { get; set; }

        public string? SKUCode { get; set; }

        public double OriginalPrice { get; set; }

        public double MedicineDiscountAmount { get; set; }

        public double FinalPrice { get; set; }
    }
}
