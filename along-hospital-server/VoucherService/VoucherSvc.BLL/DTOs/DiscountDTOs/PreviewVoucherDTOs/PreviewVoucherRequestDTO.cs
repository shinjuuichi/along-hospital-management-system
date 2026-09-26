namespace VoucherSvc.BLL.DTOs.DiscountDTOs.PreviewVoucherDTOs
{
    public class PreviewVoucherRequestDTO
    {
        public int PatientId { get; set; }

        public string? VoucherCode { get; set; }

        public List<MedicineItemDTO> Medicines { get; set; } = [];
    }
}
