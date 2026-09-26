using FeedbackSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace FeedbackSvc.BLL.DTOs.FeedbackRespondDTOs
{
    public class CreateFeedbackRespondDTO : MapTo<FeedbackRespond>
    {
        public string? Content { get; set; }

        public int FeedbackId { get; set; }
    }
}