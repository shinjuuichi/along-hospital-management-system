using RecruitmentSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace RecruitmentSvc.BLL.DTOs.InterviewDTOs
{
    public class GetInterviewDTO : MapFrom<Interview>
    {
        public int Id { get; set; }

        public DateTime? InterviewDate { get; set; }

        public string? Result { get; set; }

        public string? Note { get; set; }

        public int JobApplicationId { get; set; }

        public string? JobApplicationStatus { get; set; }

        public int InterviewTypeId { get; set; }

        public string? InterviewTypeName { get; set; }

        public string? ApplicationName { get; set; }

        public string? ApplicationEmail { get; set; }

        public string? ApplicationPhone { get; set; }

        public string? CVUrl { get; set; }
    }
}