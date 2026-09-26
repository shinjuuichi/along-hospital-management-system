using MedicalOrderSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace MedicalOrderSvc.BLL.DTOs.MedicalOrderDTOs
{
    public class CreateMedicalOrderDTO : MapTo<MedicalOrder>
    {
        public string? MedicalOrderType { get; set; }

        public string? Instruction { get; set; }

        public int MedicalHistoryId { get; set; }
    }
}