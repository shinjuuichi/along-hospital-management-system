using FeedbackSvc.DAL.Enums;
using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;

namespace FeedbackSvc.DAL.Models
{
    public class FeedbackReport : AuditEntity
    {
        [MessageRequired, MessageMaxLength(500)]
        public string Reason { get; set; } = string.Empty;

        public FeedbackReportStatusEnum Status { get; set; } = FeedbackReportStatusEnum.Pending;

        public int FeedbackId { get; set; }

        public virtual Feedback? Feedback { get; set; }
    }
}