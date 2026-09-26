using FeedbackSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace FeedbackSvc.BLL.DTOs.FeedbackDTOs
{
    public class UpdateFeedbackDTO : MapTo<Feedback>
    {
        public string? Content { get; set; }

        public double Rating { get; set; }

        public int MedicineId { get; set; }
    }
}