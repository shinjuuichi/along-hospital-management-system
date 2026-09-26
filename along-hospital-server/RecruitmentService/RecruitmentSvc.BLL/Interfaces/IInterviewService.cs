using RecruitmentSvc.BLL.DTOs.InterviewDTOs;
using SharedLibrary.Base.Services;

namespace RecruitmentSvc.BLL.Interfaces
{
    public interface IInterviewService : IBaseCrudService<CreateInterviewDTO, UpdateInterviewDTO, GetInterviewDTO>
    {
        Task<List<GetInterviewDTO>> GetAllByJobApplicationIdAsync(int jobApplicationId);
    }
}