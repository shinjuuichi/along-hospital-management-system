using FeedbackSvc.BLL.DTOs.FeedbackReportDTOs;
using FeedbackSvc.DAL.Enums;
using SharedLibrary.Base.Services;

namespace FeedbackSvc.BLL.Interfaces
{
    public interface IFeedbackReportService : IBaseCrudService<CreateFeedbackReportDTO, NoUpdateFeedbackReportDTO, GetFeedbackReportDTO>
    {
        Task UpdateStatusAsync(int id, FeedbackReportStatusEnum feedbackReportStatus);
    }
}