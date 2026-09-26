using BillingSvc.DAL.Enums;
using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;

namespace BillingSvc.DAL.Models
{
    public class Refund : AuditEntity
    {
        [MessageRequired, MessageMaxLength(1000)]
        public string Reason { get; set; } = string.Empty;

        public RefundStatusEnum RefundStatus { get; set; } = RefundStatusEnum.Pending;

        public DateTime? ApprovalDate { get; set; }

        public int? ApprovedBy { get; set; }

        [MessageRequired, MessageMaxLength(50)]
        public string ClinicalMedicalOrderDetailId { get; set; } = string.Empty;

        public int ChargeId { get; set; }

        public virtual Charge? Charge { get; set; }
    }
}
