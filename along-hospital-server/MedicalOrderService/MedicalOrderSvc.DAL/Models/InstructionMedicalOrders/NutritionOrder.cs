using MedicalOrderSvc.DAL.Enums;
using SharedLibrary.Commons.EntityAbstractions.Mongo;
using SharedLibrary.Commons.EntityAnnotations;

namespace MedicalOrderSvc.DAL.Models.InstructionMedicalOrders
{
    public class NutritionOrder : MongoAuditEntity
    {
        public NutritionOrderTypeEnum NutritionOrderType { get; set; } = NutritionOrderTypeEnum.Other;

        [MessageMaxLength(500)]
        public string? Instruction { get; set; }
    }
}
