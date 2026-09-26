using SharedLibrary.Commons.EntityAnnotations;

namespace MedicalOrderSvc.DAL.Models.InfusionMedicalOrders
{
    [CollectionName(nameof(MedicalOrder))]
    public class InfusionMedicalOrder : MedicalOrder
    {
        public virtual ICollection<InfusionMedicalOrderDetail> InfusionMedicalOrderDetails { get; set; } = [];
    }
}