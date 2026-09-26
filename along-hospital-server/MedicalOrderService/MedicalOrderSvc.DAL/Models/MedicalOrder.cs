using MedicalOrderSvc.DAL.Enums;
using MedicalOrderSvc.DAL.Models.ClinicalMedicalOrders;
using MedicalOrderSvc.DAL.Models.InfusionMedicalOrders;
using MedicalOrderSvc.DAL.Models.InstructionMedicalOrders;
using MongoDB.Bson.Serialization.Attributes;
using SharedLibrary.Commons.EntityAbstractions.Mongo;
using SharedLibrary.Commons.EntityAnnotations;

namespace MedicalOrderSvc.DAL.Models
{
    [CollectionName("MedicalOrder")]
    [BsonDiscriminator(RootClass = true)]
    [BsonKnownTypes(
        typeof(ClinicalMedicalOrder),
        typeof(InfusionMedicalOrder),
        typeof(InstructionMedicalOrder))]
    public class MedicalOrder : MongoAuditEntity
    {
        [MessageRequired]
        public MedicalOrderTypeEnum MedicalOrderType { get; set; }

        [MessageMaxLength(1000)]
        public string? Instruction { get; set; }

        [MessageRequired]
        public int MedicalHistoryId { get; set; }
    }
}