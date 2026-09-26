using MedicalOrderSvc.DAL.Models.InstructionMedicalOrders;
using SharedLibrary.Base.Mappers;

namespace MedicalOrderSvc.BLL.DTOs.InstructionMedicalOrderDTOs.DetailDTOs
{
    public class UpsertNutritionOrderDTO : MapTo<NutritionOrder>
    {
        public string? NutritionOrderType { get; set; }

        public string? Instruction { get; set; }
    }
}