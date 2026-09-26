using MedicalOrderSvc.DAL.Enums;
using MedicalOrderSvc.DAL.Models.Snapshots;
using SharedLibrary.Commons.EntityAbstractions.Mongo;
using SharedLibrary.Commons.EntityAnnotations;

namespace MedicalOrderSvc.DAL.Models.InfusionMedicalOrders
{
    public class InfusionMedicalOrderDetail : MongoAuditEntity
    {
        [MessageRequired, MessageMaxLength(100)]
        public string Rate { get; set; } = string.Empty;

        [MessageRequired, MessageMaxLength(100)]
        public string Frequency { get; set; } = string.Empty;

        [MessageRequired, MessageMaxLength(100)]
        public string Duration { get; set; } = string.Empty;

        public InfusionMedicalOrderDetailExecutionStatusEnum InfusionMedicalOrderDetailExecutionStatus { get; set; }
            = InfusionMedicalOrderDetailExecutionStatusEnum.Pending;

        [MessageMaxLength(1000)]
        public string? Note { get; set; }

        public int MedicineId { get; set; }

        public virtual MedicineSnapshot? MedicineSnapshot { get; set; }
    }
}