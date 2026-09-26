using MedicalOrderSvc.DAL.Models.InstructionMedicalOrders;
using SharedLibrary.Base.Mappers;

namespace MedicalOrderSvc.BLL.DTOs.InstructionMedicalOrderDTOs.DetailDTOs
{
    public class UpsertNursingCareOrderDTO : MapTo<NursingCareOrder>
    {
        public string? NursingCareOrderLevel { get; set; }

        public int MonitorIntervalHour { get; set; }
    }
}
