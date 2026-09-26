using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;

namespace RecruitmentSvc.DAL.Models
{
    public class InterviewType : AuditEntity
    {
        [MessageRequired]
        [MessageMaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MessageMaxLength(500)]
        public string? Description { get; set; }

        public virtual ICollection<Interview> Interviews { get; set; } = [];
    }
}