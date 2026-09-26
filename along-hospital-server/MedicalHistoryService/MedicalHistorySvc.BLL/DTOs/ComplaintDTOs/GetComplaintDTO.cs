using MedicalHistorySvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace MedicalHistorySvc.BLL.DTOs.ComplaintDTOs
{
    public class GetComplaintDTO : MapFrom<Complaint>
    {
        public int Id { get; set; }

        public string? ComplaintTopic { get; set; }

        public string? Content { get; set; }

        public string? Response { get; set; }

        public string? ComplaintType { get; set; }

        public string? ComplaintResolveStatus { get; set; }

        public int MedicalHistoryId { get; set; }
    }
}