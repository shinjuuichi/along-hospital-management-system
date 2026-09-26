using FeedbackSvc.BLL.DTOs.FeedbackDTOs;
using SharedLibrary.Base.Services;

namespace FeedbackSvc.BLL.Interfaces
{
    public interface IFeedbackService : IBaseCrudService<CreateFeedbackDTO, UpdateFeedbackDTO, GetFeedbackDTO>
    {
        Task<List<GetFeedbackDTO>> GetFeedbackByMedicineIdAsync(int medicineId);
        Task BanFeedbackByUserIdAsync(int userId);
        Task BanFeedbackAsync(int id);
        Task UpdateFeedbackTypeAsync(int id, string feedbackType);
        Task UpdateFeedbackReplyStatusAsync(int id, string feedbackReplyStatus);
    }
}