using MedicalHistorySvc.BLL.DTOs.MedicalHistoryDTOs;
using MedicalHistorySvc.BLL.DTOs.MedicalHistoryDTOs.GetDTOs;
using MedicalHistorySvc.BLL.DTOs.MedicalHistoryDTOs.GetDTOs.MedicalOrderDTOs;
using MedicalHistorySvc.BLL.DTOs.PrescriptionDTOs.UpsertDTOs;
using MedicalHistorySvc.BLL.DTOs.StatisticsDTOs;
using MedicalHistorySvc.DAL.Models.Snapshots;
using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Contracts.BillingContracts;
using MessageBroker.Contracts.InPatientResourceContracts;
using MessageBroker.Contracts.MedicalHistoryContracts;
using MessageBroker.Contracts.MedicalOrderContracts;
using MessageBroker.Contracts.MedicineContracts;
using MessageBroker.Contracts.StaffContracts.SpecialtyContracts;
using MessageBroker.Events.BillingEvents;
using MessageBroker.Events.MedicalHistoryEvents;
using SharedLibrary.Base.Mappers;
using System.Reflection;

namespace MedicalHistorySvc.BLL
{
    public class MedicalHistoryMappingProfile : BaseMappingProfile
    {
        public MedicalHistoryMappingProfile() : base(Assembly.GetExecutingAssembly())
        {
            // Contract, DTO
            CreateMap<MedicalHistoryStatisticsDTO, GetMedicalHistoryStatisticsByDateRangeContract>();
            CreateMap<GetMedicalHistoryDTO, GetMedicalHistoryContract>();
            CreateMap<GetMedicalHistoryDTO, GetMedicalHistoryOccupancySummaryContract>()
                .ForMember(d => d.PatientName, opt => opt.MapFrom(s => s.Patient != null ? s.Patient.Name : null))
                .ForMember(d => d.DoctorName, opt => opt.MapFrom(s => s.Doctor != null ? s.Doctor.Name : null));
            CreateMap<GetMedicineByIdContract, UpsertPrescriptionDetailMedicineSnapshotDTO>()
                .ForMember(d => d.MedicineName, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.MedicineBrand, opt => opt.MapFrom(s => s.Brand))
                .ForMember(d => d.MedicineImage, opt => opt.MapFrom(s => s.MedicineImages.FirstOrDefault()));
            CreateMap<GetSpecialtyByIdContract, GetMedicalHistorySpecialtyDTO>();

            // Event, DTO
            CreateMap<CreateMedicalHistoryFromAppointmentEvent, CreateMedicalHistoryDTO>();
            CreateMap<CreateBedChargeInvoiceWhenMedicalHistoryDischargedDTO,
                CreateBedChargeInvoiceWhenMedicalHistoryDischargedEvent>();
            CreateMap<CreateBedChargeInvoiceWhenMedicalHistoryDischargedChargeItemDTO,
                CreateBedChargeInvoiceWhenMedicalHistoryDischargedEvent
                    .CreateBedChargeInvoiceChargeItemEvent>();

            // Additional mappings
            MapUserContract();
            MapInvoiceContract();
            MapBedOccupancyContract();
            MapMedicalOrderContract();
        }

        private void MapUserContract()
        {
            CreateMap<GetPatientAllergyDataContractItem, GetMedicalHistoryPatientAllergyDTO>();
            CreateMap<GetPatientDataByUserIdContract, GetMedicalHistoryPatientDTO>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.UserId));
            CreateMap<GetStaffDataByUserIdContract, GetMedicalHistoryStaffDTO>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.UserId));

            CreateMap<GetPatientDataByUserIdContract, PatientSnapshot>();
            CreateMap<GetPatientAllergyDataContractItem, AllergySnapshot>();
        }

        private void MapInvoiceContract()
        {
            CreateMap<GetInvoiceContract,
                        GetMedicalHistoryInvoiceDTO>();

            CreateMap<GetInvoiceContract.GetChargeContract,
                            GetMedicalHistoryInvoiceChargeDTO>();

            CreateMap<GetInvoiceContract.GetChargeContract.GetChargeSnapshotContract,
                        GetMedicalHistoryInvoiceChargeSnapshotDTO>();

            CreateMap<GetInvoiceContract.GetChargeContract.GetRefundContract,
                        GetMedicalHistoryInvoiceChargeRefundDTO>();

            CreateMap<GetStaffDataByUserIdContract,
                        GetMedicalHistoryInvoiceChargeRefundStaffDTO>()
                        .ForMember(d => d.Id, opt => opt.MapFrom(s => s.UserId));
        }

        private void MapBedOccupancyContract()
        {
            CreateMap<GetBedOccupancyByMedicalHistoryIdContract, GetMedicalHistoryBedOccupancyDTO>();
            CreateMap<GetBedContract, GetMedicalHistoryBedDTO>();
            CreateMap<GetRoomContract, GetMedicalHistoryRoomDTO>();
        }

        private void MapMedicalOrderContract()
        {
            CreateMap<GetMedicalOrderContract, GetMedicalHistoryMedicalOrderDTO>();

            CreateMap<GetClinicalMedicalOrderContract, GetMedicalHistoryClinicalMedicalOrderDTO>();
            CreateMap<
                GetClinicalMedicalOrderDetailContract,
                    GetMedicalHistoryClinicalMedicalOrderDTO
                    .ClinicalMedicalOrderDetailDTO>();
            CreateMap<
                GetClinicalMedicalOrderDetailContract
                .GetMedicalServiceSnapshotContract,
                    GetMedicalHistoryClinicalMedicalOrderDTO
                    .ClinicalMedicalOrderDetailDTO
                    .MedicalServiceSnapshotDTO>();

            CreateMap<GetInfusionMedicalOrderContract, GetMedicalHistoryInfusionMedicalOrderDTO>();
            CreateMap<
                GetInfusionMedicalOrderDetailContract,
                    GetMedicalHistoryInfusionMedicalOrderDTO
                    .InfusionMedicalOrderDetailDTO>();
            CreateMap<
                GetInfusionMedicalOrderDetailContract
                .GetMedicineSnapshotContract,
                    GetMedicalHistoryInfusionMedicalOrderDTO
                    .InfusionMedicalOrderDetailDTO
                    .MedicineSnapshotDTO>();

            CreateMap<GetInstructionMedicalOrderContract, GetMedicalHistoryInstructionMedicalOrderDTO>();
            CreateMap<GetPositionOrderContract, GetMedicalHistoryInstructionMedicalOrderDTO.PositionOrderDTO>();
            CreateMap<GetRespiratorySupportOrderContract, GetMedicalHistoryInstructionMedicalOrderDTO.RespiratorySupportOrderDTO>();
            CreateMap<GetNutritionOrderContract, GetMedicalHistoryInstructionMedicalOrderDTO.NutritionOrderDTO>();
            CreateMap<GetNursingCareOrderContract, GetMedicalHistoryInstructionMedicalOrderDTO.NursingCareOrderDTO>();
        }
    }
}
