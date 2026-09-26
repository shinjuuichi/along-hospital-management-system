namespace MedicalHistorySvc.BLL.DTOs.MedicalHistoryDTOs.GetDTOs
{
    public class GetMedicalHistoryInvoiceDTO
    {
        public int Id { get; set; }

        public string? InvoiceNumber { get; set; }

        public string? InvoiceStatus { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? PaymentDate { get; set; }

        public double TotalInvoiceAmount { get; set; }

        public double TotalRefundAmount { get; set; }

        public double TotalAmount { get; set; }

        public int MedicalHistoryId { get; set; }

        public List<GetMedicalHistoryInvoiceChargeDTO> Charges { get; set; } = [];
    }

    public class GetMedicalHistoryInvoiceChargeDTO
    {
        public int Id { get; set; }

        public int Quantity { get; set; }

        public double UnitPrice { get; set; }

        public string? ChargeType { get; set; }

        public double TotalAmount { get; set; }

        public int InvoiceId { get; set; }

        public int MedicalServiceId { get; set; }

        public GetMedicalHistoryInvoiceChargeSnapshotDTO? ChargeSnapshot { get; set; }

        public GetMedicalHistoryInvoiceChargeRefundDTO? Refund { get; set; }
    }

    public class GetMedicalHistoryInvoiceChargeSnapshotDTO
    {
        public string? MedicalServiceName { get; set; }

        public string? MedicalServiceCode { get; set; }

        public string? MedicalServiceDescription { get; set; }
    }

    public class GetMedicalHistoryInvoiceChargeRefundDTO
    {
        public int Id { get; set; }

        public string? Reason { get; set; }

        public string? RefundStatus { get; set; }

        public int? ApprovedBy { get; set; }

        public DateTime? ApprovalDate { get; set; }

        public int ChargeId { get; set; }

        public GetMedicalHistoryInvoiceChargeRefundStaffDTO? Staff { get; set; }
    }

    public class GetMedicalHistoryInvoiceChargeRefundStaffDTO
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Image { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? Role { get; set; }
    }
}
