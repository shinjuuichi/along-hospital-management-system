namespace VoucherSvc.BLL.DTOs.DiscountDTOs.ApplyVoucherDTOs
{
    public class ApplyVoucherRequestDTO
    {
        public int PatientId { get; set; }

        public string? VoucherCode { get; set; }

        public List<MedicineItemDTO> Medicines { get; set; } = [];
    }
}