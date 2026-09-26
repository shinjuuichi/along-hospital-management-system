using BillingSvc.DAL.Enums;
using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;

namespace BillingSvc.DAL.Models
{
    public class Invoice : AuditEntity
    {
        [MessageRequired]
        [MessageMaxLength(50)]
        [Unique]
        public string InvoiceNumber { get; set; } = string.Empty;

        public InvoiceStatusEnum InvoiceStatus { get; set; } = InvoiceStatusEnum.Pending;

        public Guid? TransactionId { get; set; }

        public DateTime? PaymentDate { get; set; }

        [MessageRequired]
        public int MedicalHistoryId { get; set; }

        [MessageMaxLength(50)]
        public string? ClinicalMedicalOrderId { get; set; }

        public virtual ICollection<Charge> Charges { get; set; } = [];

        public double GetTotalInvoiceAmount()
            => Charges
            .Where(c => c.ChargeType == ChargeTypeEnum.Invoice)
            .Sum(c => c.GetTotalAmount());

        public double GetTotalRefundAmount()
            => Charges
            .Where(c => c.ChargeType == ChargeTypeEnum.Refund)
            .Sum(c => c.GetTotalAmount());

        public double GetTotalAmount()
            => GetTotalInvoiceAmount() - GetTotalRefundAmount();
    }
}