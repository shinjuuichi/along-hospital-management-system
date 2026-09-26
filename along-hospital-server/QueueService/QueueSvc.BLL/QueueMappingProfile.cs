using MessageBroker.Contracts.AppointmentContracts;
using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Contracts.InPatientResourceContracts;
using MessageBroker.Contracts.MedicalHistoryContracts;
using MessageBroker.Contracts.StaffContracts.SpecialtyContracts;
using QueueSvc.BLL.DTOs.CreateQueueDTOs;
using QueueSvc.BLL.DTOs.OtherDTOs;
using SharedLibrary.Base.Mappers;
using System.Reflection;

namespace QueueSvc.BLL
{
    public class QueueMappingProfile : BaseMappingProfile
    {
        public QueueMappingProfile() : base(Assembly.GetExecutingAssembly())
        {
            CreateMap<GetAppointmentContract, CreateQueueFromAppointmentDataDTO>()
                .ForMember(dest => dest.AppointmentId, opt => opt.MapFrom(src => src.Id));
            CreateMap<GetPatientDataByUserIdContract, GetPatientDTO>();
            CreateMap<GetRoomContract, GetRoomDTO>();
            CreateMap<GetStaffDataByUserIdContract, GetStaffDTO>();

            MapSnapshotFromContract();
        }

        private void MapSnapshotFromContract()
        {
            CreateMap<GetAppointmentContract, CreateQueueFromAppointmentDataDTO.CreateAppointmentSnapshotDTO>();
            CreateMap<GetMedicalHistoryContract, CreateMedicalHistorySnapshotDTO>();
            CreateMap<GetSpecialtyByIdContract, CreateSpecialtySnapshotDTO>();
        }
    }
}