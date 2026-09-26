using RecruitmentSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace RecruitmentSvc.BLL.DTOs.InterviewDTOs
{
    public class UpdateInterviewDTO : MapTo<Interview>
    {
        public DateTime? InterviewDate { get; set; }

        public int InterviewTypeId { get; set; }

        public string? Result { get; set; }

        public string? Note { get; set; }
    }
}