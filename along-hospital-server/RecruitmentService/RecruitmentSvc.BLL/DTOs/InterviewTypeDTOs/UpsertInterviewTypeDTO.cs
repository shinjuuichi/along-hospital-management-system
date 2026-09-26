using RecruitmentSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace RecruitmentSvc.BLL.DTOs.InterviewTypeDTOs
{
    public class UpsertInterviewTypeDTO : MapTo<InterviewType>
    {
        public string? Name { get; set; }

        public string? Description { get; set; }
    }
}