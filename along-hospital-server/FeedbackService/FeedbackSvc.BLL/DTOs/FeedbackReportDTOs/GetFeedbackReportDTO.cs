using FeedbackSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace FeedbackSvc.BLL.DTOs.FeedbackReportDTOs
{
    public class GetFeedbackReportDTO : MapFrom<FeedbackReport>
    {
        public int Id { get; set; }

        public int FeedbackId { get; set; }

        public string? FeedbackContent { get; set; }

        public string? Reason { get; set; }

        public string? Status { get; set; }
    }
}