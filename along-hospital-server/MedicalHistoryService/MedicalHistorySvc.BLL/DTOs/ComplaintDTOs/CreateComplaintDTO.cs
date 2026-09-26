using MedicalHistorySvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace MedicalHistorySvc.BLL.DTOs.ComplaintDTOs
{
    public class CreateComplaintDTO : MapTo<Complaint>
    {
        public string? ComplaintTopic { get; set; }

        public string? Content { get; set; }
    }
}
