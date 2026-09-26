using RecruitmentSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace RecruitmentSvc.BLL.DTOs.JobPostingDTOs
{
    public class UpsertJobPostingDTO : MapTo<JobPosting>
    {
        public string? Title { get; set; }

        public string? Role { get; set; }

        public string? Description { get; set; }

        public string? Requirement { get; set; }

        public string? Benefit { get; set; }

        public string? EmploymentType { get; set; }

        public double? SalaryMin { get; set; }

        public DateOnly? CloseDate { get; set; }
    }
}
