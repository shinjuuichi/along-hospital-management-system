using RecruitmentSvc.DAL.Enums;
using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.Commons.EntityAnnotations.DateAttributes;
using SharedLibrary.Commons.EntityAnnotations.RegExAttributes;

namespace RecruitmentSvc.DAL.Models
{
    public class JobApplication : BaseEntity
    {
        [MessageRequired]
        [MessageMaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MessageRequired]
        [EmailValidator]
        [MessageMaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [MessageRequired]
        [PhoneNumberValidator]
        [MessageMaxLength(15)]
        public string Phone { get; set; } = string.Empty;

        [MessageRequired]
        [AgeRange(18, 65)]
        public DateOnly DateOfBirth { get; set; }

        [MessageMaxLength(500)]
        public string? Address { get; set; }

        [MessageRequired]
        public GenderEnum Gender { get; set; }

        public DateOnly ApplyDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

        public JobApplicationStatusEnum ApplicationStatus { get; set; } = JobApplicationStatusEnum.Applied;

        [MessageRequired]
        public string CVUrl { get; set; } = string.Empty;

        [MessageRequired]
        public int JobPostingId { get; set; }

        public virtual JobPosting? JobPosting { get; set; }
        public virtual ICollection<Interview> Interviews { get; set; } = [];
    }
}