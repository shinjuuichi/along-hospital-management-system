using MedicalOrderSvc.BLL.DTOs.ClinicalMedicalOrderDTOs;
using MedicalOrderSvc.BLL.DTOs.InfusionMedicalOrderDTOs;
using MedicalOrderSvc.BLL.DTOs.InstructionMedicalOrderDTOs;
using MedicalOrderSvc.DAL.Models.Snapshots;
using MessageBroker.Contracts.MedicalOrderContracts;
using MessageBroker.Contracts.MedicalServiceContracts;
using MessageBroker.Contracts.MedicineContracts;
using SharedLibrary.Base.Mappers;
using System.Reflection;

namespace MedicalOrderSvc.BLL
{
    public class MedicalOrderMappingProfile : BaseMappingProfile
    {
        public MedicalOrderMappingProfile() : base(Assembly.GetExecutingAssembly())
        {
            MapSnapshot();
            MapInfusionMedicalOrder();
            MapClinicalMedicalOrder();
            MapInstructionMedicalOrder();
        }

        private void MapSnapshot()
        {
            CreateMap<GetMedicalServiceContract, MedicalServiceSnapshot>();
            CreateMap<GetMedicineByIdContract, MedicineSnapshot>()
                .ForMember(dest => dest.MedicineImage, opt => opt.MapFrom(src => src.MedicineImages.FirstOrDefault()));
        }

        private void MapInfusionMedicalOrder()
        {
            CreateMap<GetInfusionMedicalOrderDTO, GetInfusionMedicalOrderContract>();
            CreateMap<GetInfusionMedicalOrderDetailDTO, GetInfusionMedicalOrderDetailContract>();
            CreateMap<GetMedicineSnapshotDTO, GetInfusionMedicalOrderDetailContract.GetMedicineSnapshotContract>();
        }

        private void MapClinicalMedicalOrder()
        {
            CreateMap<GetClinicalMedicalOrderDTO, GetClinicalMedicalOrderContract>();
            CreateMap<GetClinicalMedicalOrderDetailDTO, GetClinicalMedicalOrderDetailContract>();
            CreateMap<GetMedicalServiceSnapshotDTO, GetClinicalMedicalOrderDetailContract.GetMedicalServiceSnapshotContract>();
        }

        private void MapInstructionMedicalOrder()
        {
            CreateMap<GetInstructionMedicalOrderDTO, GetInstructionMedicalOrderContract>();
            CreateMap<GetPositionOrderDTO, GetPositionOrderContract>();
            CreateMap<GetRespiratorySupportOrderDTO, GetRespiratorySupportOrderContract>();
            CreateMap<GetNutritionOrderDTO, GetNutritionOrderContract>();
            CreateMap<GetNursingCareOrderDTO, GetNursingCareOrderContract>();
        }
    }
}