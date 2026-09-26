using MedicalOrderSvc.DAL.Enums;
using SharedLibrary.Commons.EntityAbstractions.Mongo;
using SharedLibrary.Commons.EntityAnnotations;

namespace MedicalOrderSvc.DAL.Models.InstructionMedicalOrders
{
    public class PositionOrder : MongoAuditEntity
    {
        public PositionOrderTypeEnum PositionOrderType { get; set; } = PositionOrderTypeEnum.Other;

        [MessageMaxLength(500)]
        public string? Instruction { get; set; }
    }
}
