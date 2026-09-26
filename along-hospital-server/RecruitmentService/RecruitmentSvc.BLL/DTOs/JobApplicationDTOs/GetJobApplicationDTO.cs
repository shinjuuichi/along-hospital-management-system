using RecruitmentSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace RecruitmentSvc.BLL.DTOs.JobApplicationDTOs
{
    public class GetJobApplicationDTO : MapFrom<JobApplication>
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public string? Address { get; set; }

        public string? Gender { get; set; }

        public DateOnly? ApplyDate { get; set; }

        public string? ApplicationStatus { get; set; }

        public string? CVUrl { get; set; }

        public int JobPostingId { get; set; }
    }
}