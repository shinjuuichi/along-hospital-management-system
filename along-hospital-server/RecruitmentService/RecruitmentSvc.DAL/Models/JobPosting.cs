using RecruitmentSvc.DAL.Enums;
using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecruitmentSvc.DAL.Models
{
    public class JobPosting : AuditEntity
    {
        [MessageRequired]
        [MessageMaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MessageRequired]
        public RoleEnum Role { get; set; }

        [MessageRequired]
        [Column(TypeName = "nvarchar(max)")]
        public string Description { get; set; } = string.Empty;

        [MessageRequired]
        [Column(TypeName = "nvarchar(max)")]
        public string Requirement { get; set; } = string.Empty;

        [MessageRequired]
        [Column(TypeName = "nvarchar(max)")]
        public string Benefit { get; set; } = string.Empty;

        [MessageRequired]
        public EmploymentTypeEnum EmploymentType { get; set; }

        [MessageRequired]
        [NumberPositive]
        public double SalaryMin { get; set; }

        public DateOnly? CloseDate { get; set; }

        public JobPostingStatusEnum Status { get; set; } = JobPostingStatusEnum.Draft;

        public virtual ICollection<JobApplication> JobApplications { get; set; } = [];
    }
}