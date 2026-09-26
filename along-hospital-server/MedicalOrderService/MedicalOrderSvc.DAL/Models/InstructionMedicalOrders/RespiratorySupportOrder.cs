using MedicalOrderSvc.DAL.Enums;
using SharedLibrary.Commons.EntityAbstractions.Mongo;
using SharedLibrary.Commons.EntityAnnotations;

namespace MedicalOrderSvc.DAL.Models.InstructionMedicalOrders
{
    public class RespiratorySupportOrder : MongoAuditEntity
    {
        public RespiratorySupportOrderTypeEnum RespiratorySupportOrderType { get; set; }
            = RespiratorySupportOrderTypeEnum.Other;

        [NumberHigherThanOrEqualTo(0.1)]
        public double OxygenFlow { get; set; }

        [MessageRange(0.21, 1.0)]
        public double FiO2 { get; set; }

        [MessageMaxLength(500)]
        public string? Instruction { get; set; }
    }
}
