using AutoMapper;
using MedicalOrderSvc.BLL.DTOs.ClinicalMedicalOrderDTOs;
using MedicalOrderSvc.BLL.DTOs.InfusionMedicalOrderDTOs;
using MedicalOrderSvc.BLL.DTOs.InstructionMedicalOrderDTOs;
using MedicalOrderSvc.BLL.DTOs.MedicalOrderDTOs;
using SharedLibrary.Base.Mappers;
using SharedLibrary.Commons.EntityAbstractions;

namespace MedicalOrderSvc.BLL.DTOs
{
    public class CreateMedicalOrderWrapperDTO : MapTo<Entity>
    {
        public string? MedicalOrderType { get; set; }

        public string? Instruction { get; set; }

        public int MedicalHistoryId { get; set; }

        public CreateInstructionMedicalOrderDTO? InstructionMedicalOrder { get; set; }

        public CreateInfusionMedicalOrderDTO? InfusionMedicalOrder { get; set; }

        public CreateClinicalMedicalOrderDTO? ClinicalMedicalOrder { get; set; }

        public override void Mapping(Profile profile)
        {
            profile.CreateMap<CreateMedicalOrderWrapperDTO, CreateMedicalOrderDTO>()
                .ForMember(dest => dest.Instruction, opt => opt.MapFrom(src => src.Instruction))
                .ForMember(dest => dest.MedicalHistoryId, opt => opt.MapFrom(src => src.MedicalHistoryId))
                .ForMember(dest => dest.MedicalOrderType, opt => opt.MapFrom(src => src.MedicalOrderType))
                .Include<CreateMedicalOrderWrapperDTO, CreateInstructionMedicalOrderDTO>()
                .Include<CreateMedicalOrderWrapperDTO, CreateInfusionMedicalOrderDTO>()
                .Include<CreateMedicalOrderWrapperDTO, CreateClinicalMedicalOrderDTO>();

            profile.CreateMap<CreateMedicalOrderWrapperDTO, CreateInstructionMedicalOrderDTO>()
                .IncludeBase<CreateMedicalOrderWrapperDTO, CreateMedicalOrderDTO>();

            profile.CreateMap<CreateMedicalOrderWrapperDTO, CreateInfusionMedicalOrderDTO>()
                .IncludeBase<CreateMedicalOrderWrapperDTO, CreateMedicalOrderDTO>();

            profile.CreateMap<CreateMedicalOrderWrapperDTO, CreateClinicalMedicalOrderDTO>()
                .IncludeBase<CreateMedicalOrderWrapperDTO, CreateMedicalOrderDTO>();
        }
    }
}
