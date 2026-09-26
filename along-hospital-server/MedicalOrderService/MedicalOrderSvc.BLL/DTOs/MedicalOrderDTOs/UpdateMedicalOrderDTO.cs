using MedicalOrderSvc.DAL.Models;
using SharedLibrary.Base.Mappers;

namespace MedicalOrderSvc.BLL.DTOs.MedicalOrderDTOs
{
    public class UpdateMedicalOrderDTO : MapTo<MedicalOrder>
    {
        public string? Instruction { get; set; }
    }
}