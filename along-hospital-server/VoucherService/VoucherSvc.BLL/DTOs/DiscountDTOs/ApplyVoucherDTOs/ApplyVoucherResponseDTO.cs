namespace VoucherSvc.BLL.DTOs.DiscountDTOs.ApplyVoucherDTOs
{
    public class ApplyVoucherResponseDTO
    {
        public double OriginalTotalPrice { get; set; }

        public double FinalTotalPrice { get; set; }

        public List<MedicineDiscountDetailDTO> MedicineDiscounts { get; set; } = [];

        public double PatientVoucherDiscountAmount { get; set; }

        public string? PatientVoucherCode { get; set; }

        public double TotalDiscountAmount => OriginalTotalPrice - FinalTotalPrice;
    }
}
