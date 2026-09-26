using FeedbackSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace FeedbackSvc.BLL.DTOs.FeedbackRespondDTOs
{
    public class UpdateFeedbackRespondDTO : MapTo<FeedbackRespond>
    {
        public string? Content { get; set; }
    }
}