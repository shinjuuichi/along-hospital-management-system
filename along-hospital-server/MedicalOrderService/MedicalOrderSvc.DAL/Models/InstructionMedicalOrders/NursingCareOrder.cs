using MedicalOrderSvc.DAL.Enums;
using SharedLibrary.Commons.EntityAbstractions.Mongo;
using SharedLibrary.Commons.EntityAnnotations;

namespace MedicalOrderSvc.DAL.Models.InstructionMedicalOrders
{
    public class NursingCareOrder : MongoAuditEntity
    {
        public NursingCareOrderLevelEnum NursingCareOrderLevel { get; set; }
            = NursingCareOrderLevelEnum.Other;

        [NumberHigherThanOrEqualTo(1)]
        public int MonitorIntervalHour { get; set; }
    }
}