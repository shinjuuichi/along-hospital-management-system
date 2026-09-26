using RecruitmentSvc.DAL.Enums;
using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;

namespace RecruitmentSvc.DAL.Models
{
    public class Interview : AuditEntity
    {
        [MessageRequired]
        public DateTime InterviewDate { get; set; }

        public InterviewResultEnum Result { get; set; } = InterviewResultEnum.Pending;

        [MessageMaxLength(1000)]
        public string? Note { get; set; }

        public int JobApplicationId { get; set; }
        public int InterviewTypeId { get; set; }

        public virtual JobApplication? JobApplication { get; set; }
        public virtual InterviewType? InterviewType { get; set; }
    }
}