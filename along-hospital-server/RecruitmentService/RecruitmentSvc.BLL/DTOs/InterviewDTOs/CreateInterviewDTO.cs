using RecruitmentSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace RecruitmentSvc.BLL.DTOs.InterviewDTOs
{
    public class CreateInterviewDTO : MapTo<Interview>
    {
        public DateTime? InterviewDate { get; set; }

        public string? Note { get; set; }

        public int JobApplicationId { get; set; }

        public int InterviewTypeId { get; set; }
    }
}
