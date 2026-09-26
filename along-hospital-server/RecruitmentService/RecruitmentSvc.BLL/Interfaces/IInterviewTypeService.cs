using RecruitmentSvc.BLL.DTOs.InterviewTypeDTOs;
using SharedLibrary.Base.Services;

namespace RecruitmentSvc.BLL.Interfaces
{
    public interface IInterviewTypeService : IBaseCrudService<UpsertInterviewTypeDTO, UpsertInterviewTypeDTO, GetInterviewTypeDTO>;
}