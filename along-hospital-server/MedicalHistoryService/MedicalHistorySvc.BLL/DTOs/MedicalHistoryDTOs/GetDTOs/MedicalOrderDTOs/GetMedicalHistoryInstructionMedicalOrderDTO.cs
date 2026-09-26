namespace MedicalHistorySvc.BLL.DTOs.MedicalHistoryDTOs.GetDTOs.MedicalOrderDTOs
{
    public class GetMedicalHistoryInstructionMedicalOrderDTO : GetMedicalHistoryMedicalOrderDTO
    {
        public string? InstructionMedicalOrderStatus { get; set; }

        public PositionOrderDTO? PositionOrder { get; set; }

        public RespiratorySupportOrderDTO? RespiratorySupportOrder { get; set; }

        public NutritionOrderDTO? NutritionOrder { get; set; }

        public NursingCareOrderDTO? NursingCareOrder { get; set; }

        public class PositionOrderDTO
        {
            public string? PositionOrderType { get; set; }

            public string? Instruction { get; set; }
        }

        public class RespiratorySupportOrderDTO
        {
            public string? RespiratorySupportOrderType { get; set; }

            public double OxygenFlow { get; set; }

            public double FiO2 { get; set; }

            public string? Instruction { get; set; }
        }

        public class NutritionOrderDTO
        {
            public string? NutritionOrderType { get; set; }

            public string? Instruction { get; set; }
        }

        public class NursingCareOrderDTO
        {
            public string? NursingCareOrderLevel { get; set; }

            public int MonitorIntervalHour { get; set; }
        }
    }
}
