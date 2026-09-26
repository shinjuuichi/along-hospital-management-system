using AutoMapper;
using MedicalOrderSvc.BLL.DTOs.MedicalOrderDTOs;
using MedicalOrderSvc.DAL.Models.InstructionMedicalOrders;
using SharedLibrary.Base.Mappers;

namespace MedicalOrderSvc.BLL.DTOs.InstructionMedicalOrderDTOs
{
    public class GetInstructionMedicalOrderDTO : GetMedicalOrderDTO, IMapFrom<InstructionMedicalOrder>
    {
        public string? InstructionMedicalOrderStatus { get; set; }

        public GetPositionOrderDTO? PositionOrder { get; set; }

        public GetRespiratorySupportOrderDTO? RespiratorySupportOrder { get; set; }

        public GetNutritionOrderDTO? NutritionOrder { get; set; }

        public GetNursingCareOrderDTO? NursingCareOrder { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<InstructionMedicalOrder, GetInstructionMedicalOrderDTO>();
        }
    }

    public class GetPositionOrderDTO : MapFrom<PositionOrder>
    {
        public string? PositionOrderType { get; set; }

        public string? Instruction { get; set; }
    }

    public class GetRespiratorySupportOrderDTO : MapFrom<RespiratorySupportOrder>
    {
        public string? RespiratorySupportOrderType { get; set; }

        public double OxygenFlow { get; set; }

        public double FiO2 { get; set; }

        public string? Instruction { get; set; }
    }

    public class GetNutritionOrderDTO : MapFrom<NutritionOrder>
    {
        public string? NutritionOrderType { get; set; }

        public string? Instruction { get; set; }
    }

    public class GetNursingCareOrderDTO : MapFrom<NursingCareOrder>
    {
        public string? NursingCareOrderLevel { get; set; }

        public int MonitorIntervalHour { get; set; }
    }
}