using FeedbackSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace FeedbackSvc.BLL.DTOs.FeedbackReportDTOs
{
    public class CreateFeedbackReportDTO : MapTo<FeedbackReport>
    {
        public int FeedbackId { get; set; }

        public string? Reason { get; set; }
    }
}