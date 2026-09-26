using AutoMapper;
using MedicalOrderSvc.BLL.DTOs.InstructionMedicalOrderDTOs.DetailDTOs;
using MedicalOrderSvc.BLL.DTOs.MedicalOrderDTOs;
using MedicalOrderSvc.DAL.Models.InstructionMedicalOrders;

namespace MedicalOrderSvc.BLL.DTOs.InstructionMedicalOrderDTOs
{
    public class CreateInstructionMedicalOrderDTO : CreateMedicalOrderDTO
    {
        public string? InstructionMedicalOrderStatus { get; set; }

        public UpsertPositionOrderDTO? PositionOrder { get; set; }

        public UpsertRespiratorySupportOrderDTO? RespiratorySupportOrder { get; set; }

        public UpsertNutritionOrderDTO? NutritionOrder { get; set; }

        public UpsertNursingCareOrderDTO? NursingCareOrder { get; set; }

        public override void Mapping(Profile profile)
        {
            profile.CreateMap<CreateInstructionMedicalOrderDTO, InstructionMedicalOrder>()
                .BeforeMap(BeforeMapping)
                .AfterMap(AfterMapping);
        }
    }
}