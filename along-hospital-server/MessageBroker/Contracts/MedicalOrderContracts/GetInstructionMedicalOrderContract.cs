namespace MessageBroker.Contracts.MedicalOrderContracts
{
    public record GetInstructionMedicalOrderContract : GetMedicalOrderContract
    {
        public string? InstructionMedicalOrderStatus { get; init; }

        public GetPositionOrderContract? PositionOrder { get; init; }

        public GetRespiratorySupportOrderContract? RespiratorySupportOrder { get; init; }

        public GetNutritionOrderContract? NutritionOrder { get; init; }

        public GetNursingCareOrderContract? NursingCareOrder { get; init; }
    }

    public record GetPositionOrderContract
    {
        public string? PositionOrderType { get; init; }

        public string? Instruction { get; init; }
    }

    public record GetRespiratorySupportOrderContract
    {
        public string? RespiratorySupportOrderType { get; init; }

        public double OxygenFlow { get; init; }

        public double FiO2 { get; init; }

        public string? Instruction { get; init; }
    }

    public record GetNutritionOrderContract
    {
        public string? NutritionOrderType { get; init; }

        public string? Instruction { get; init; }
    }

    public record GetNursingCareOrderContract
    {
        public string? NursingCareOrderLevel { get; init; }

        public int MonitorIntervalHour { get; init; }
    }
}
