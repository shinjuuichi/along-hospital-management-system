using MedicalOrderSvc.DAL.Enums;
using SharedLibrary.Commons.EntityAnnotations;

namespace MedicalOrderSvc.DAL.Models.InstructionMedicalOrders
{
    [CollectionName(nameof(MedicalOrder))]
    public class InstructionMedicalOrder : MedicalOrder
    {
        public InstructionMedicalOrderStatusEnum InstructionMedicalOrderStatus { get; set; }
            = InstructionMedicalOrderStatusEnum.Draft;

        public virtual PositionOrder? PositionOrder { get; set; }

        public virtual RespiratorySupportOrder? RespiratorySupportOrder { get; set; }

        public virtual NutritionOrder? NutritionOrder { get; set; }

        public virtual NursingCareOrder? NursingCareOrder { get; set; }
    }
}