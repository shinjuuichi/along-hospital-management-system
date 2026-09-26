using MedicalOrderSvc.DAL.Models.InstructionMedicalOrders;
using SharedLibrary.Base.Mappers;

namespace MedicalOrderSvc.BLL.DTOs.InstructionMedicalOrderDTOs.DetailDTOs
{
    public class UpsertPositionOrderDTO : MapTo<PositionOrder>
    {
        public string? PositionOrderType { get; set; }

        public string? Instruction { get; set; }
    }
}
