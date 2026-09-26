using RecruitmentSvc.BLL.DTOs.JobApplicationDTOs;
using RecruitmentSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace RecruitmentSvc.BLL.DTOs.JobPostingDTOs
{
    public class GetJobPostingDTO : MapFrom<JobPosting>
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Role { get; set; }

        public string? Description { get; set; }

        public string? Requirement { get; set; }

        public string? Benefit { get; set; }

        public string? EmploymentType { get; set; }

        public double? SalaryMin { get; set; }

        public DateOnly? CloseDate { get; set; }

        public string? Status { get; set; }

        public List<GetJobApplicationDTO> JobApplications { get; set; } = [];
    }
}