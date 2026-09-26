using MedicalOrderSvc.DAL.Enums;
using SharedLibrary.Commons.EntityAnnotations;

namespace MedicalOrderSvc.DAL.Models.ClinicalMedicalOrders
{
    [CollectionName(nameof(MedicalOrder))]
    public class ClinicalMedicalOrder : MedicalOrder
    {
        public ClinicalMedicalOrderStatusEnum ClinicalMedicalOrderStatus { get; set; }
            = ClinicalMedicalOrderStatusEnum.Pending;

        public virtual ICollection<ClinicalMedicalOrderDetail> ClinicalMedicalOrderDetails { get; set; } = [];
    }
}