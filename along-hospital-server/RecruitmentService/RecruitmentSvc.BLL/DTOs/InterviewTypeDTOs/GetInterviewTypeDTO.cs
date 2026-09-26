using RecruitmentSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace RecruitmentSvc.BLL.DTOs.InterviewTypeDTOs
{
    public class GetInterviewTypeDTO : MapFrom<InterviewType>
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }
    }
}