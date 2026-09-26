using FeedbackSvc.BLL.DTOs.FeedbackRespondDTOs;
using SharedLibrary.Base.Services;

namespace FeedbackSvc.BLL.Interfaces
{
    public interface IFeedbackRespondService : IBaseCrudService<CreateFeedbackRespondDTO, UpdateFeedbackRespondDTO, GetFeedbackRespondDTO>
    {
        Task BanFeedbackRespondAsync(int id);
    }
}
