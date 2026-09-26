namespace MedicalHistorySvc.BLL.DTOs.MedicalHistoryDTOs
{
    public class CreateBedChargeInvoiceWhenMedicalHistoryDischargedDTO
    {
        public int MedicalHistoryId { get; set; }

        public List<CreateBedChargeInvoiceWhenMedicalHistoryDischargedChargeItemDTO> Charges { get; set; } = [];
    }

    public class CreateBedChargeInvoiceWhenMedicalHistoryDischargedChargeItemDTO
    {
        public int MedicalServiceId { get; set; }

        public int Quantity { get; set; }
    }
}
