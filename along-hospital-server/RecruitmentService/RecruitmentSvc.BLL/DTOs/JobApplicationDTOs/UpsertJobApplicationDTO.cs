using Microsoft.AspNetCore.Http;
using RecruitmentSvc.DAL.Models;
using SharedLibrary.Base.Mappers;
using SharedLibrary.Commons.EntityAnnotations;
using SharedLibrary.Enums;

namespace RecruitmentSvc.BLL.DTOs.JobApplicationDTOs
{
    public class UpsertJobApplicationDTO : MapTo<JobApplication>
    {
        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public string? Address { get; set; }

        public string? Gender { get; set; }

        public string? CVUrl { get; set; }

        [AllowFileType(FileType.Custom, CustomExtensions = new[] { ".pdf" })]
        public IFormFile? CVFile { get; set; }

        public int JobPostingId { get; set; }

        public string? ApplicationStatus { get; set; }
    }
}