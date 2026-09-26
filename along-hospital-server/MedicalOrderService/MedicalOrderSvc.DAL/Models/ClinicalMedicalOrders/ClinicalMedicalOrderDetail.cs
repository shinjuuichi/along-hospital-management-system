using MedicalOrderSvc.DAL.Enums;
using MedicalOrderSvc.DAL.Models.Snapshots;
using SharedLibrary.Commons.EntityAbstractions.Mongo;
using SharedLibrary.Commons.EntityAnnotations;

namespace MedicalOrderSvc.DAL.Models.ClinicalMedicalOrders
{
    public class ClinicalMedicalOrderDetail : MongoAuditEntity
    {
        [NumberHigherThanOrEqualTo(1)]
        public int Quantity { get; set; }

        public ClinicalMedicalOrderDetailStatusEnum ClinicalMedicalOrderDetailStatus { get; set; }
            = ClinicalMedicalOrderDetailStatusEnum.Pending;

        [MessageRequired]
        public int MedicalServiceId { get; set; }

        public virtual MedicalServiceSnapshot? MedicalServiceSnapshot { get; set; }
    }
}
