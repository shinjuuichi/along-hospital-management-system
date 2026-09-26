using FeedbackSvc.DAL.Enums;
using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.Commons.EntityAnnotations.OnDeleteAttributes;

namespace FeedbackSvc.DAL.Models
{
    public class Feedback : AuditEntity
    {
        [MessageMaxLength(1000)]
        public string? Content { get; set; }

        [MessageRange(1, 5)]
        public double Rating { get; set; }

        public FeedbackTypeEnum FeedbackTypeEnum { get; set; } = FeedbackTypeEnum.Neutral;

        public FeedbackReplyStatusEnum FeedbackReplyStatus { get; set; } = FeedbackReplyStatusEnum.WaitingStaff;

        public FeedbackStatusEnum FeedbackStatus { get; set; } = FeedbackStatusEnum.Active;

        public int MedicineId { get; set; }

        [OnDelete(OnDeleteBehavior.Cascade)]
        public virtual ICollection<FeedbackReport> FeedbackReports { get; set; } = [];

        [OnDelete(OnDeleteBehavior.Cascade)]
        public virtual ICollection<FeedbackRespond> FeedbackResponds { get; set; } = [];
    }
}