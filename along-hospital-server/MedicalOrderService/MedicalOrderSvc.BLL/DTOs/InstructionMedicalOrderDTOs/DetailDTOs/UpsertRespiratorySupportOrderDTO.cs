using MedicalOrderSvc.DAL.Models.InstructionMedicalOrders;
using SharedLibrary.Base.Mappers;

namespace MedicalOrderSvc.BLL.DTOs.InstructionMedicalOrderDTOs.DetailDTOs
{
    public class UpsertRespiratorySupportOrderDTO : MapTo<RespiratorySupportOrder>
    {
        public string? RespiratorySupportOrderType { get; set; }

        public double OxygenFlow { get; set; }

        public double FiO2 { get; set; }

        public string? Instruction { get; set; }
    }
}