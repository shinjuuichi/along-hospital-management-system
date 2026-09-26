using FeedbackSvc.DAL.Enums;
using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;

namespace FeedbackSvc.DAL.Models
{
    public class FeedbackRespond : AuditEntity
    {
        [MessageRequired]
        public string Content { get; set; } = string.Empty;

        public FeedbackRespondStatusEnum FeedbackStatus { get; set; } = FeedbackRespondStatusEnum.Active;

        public int FeedbackId { get; set; }

        public virtual Feedback? Feedback { get; set; }
    }
}
