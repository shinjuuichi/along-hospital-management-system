using MessageBroker.Contracts.TeleSessionContracts;
using MessageBroker.Events.TeleHealthEvents.TeleSessionEvents;
using SharedLibrary.Base.Mappers;
using System.Reflection;
using TeleHealthSvc.BLL.DTOs.TeleRoomDTOs;
using TeleHealthSvc.BLL.DTOs.TeleSessionDTOs;
using TeleHealthSvc.DAL.Models;

namespace TeleHealthSvc.BLL
{
    public class TeleHealthMappingProfile : BaseMappingProfile
    {
        public TeleHealthMappingProfile() : base(Assembly.GetExecutingAssembly())
        {
            MappingTeleRoom();
            MappingTeleSession();
        }

        private void MappingTeleRoom()
        {
            //Entity to DTO
            CreateMap<TeleRoom, GetTeleRoomWithCredentialsDTO>()
                .ForMember(dest => dest.Credentials, opt => opt.Ignore());

            // DTO, Contract
            CreateMap<GetTeleRoomDTO, GetTeleRoomContract>();
        }

        private void MappingTeleSession()
        {
            //Event to DTO
            CreateMap<CreateTeleSessionByAppointmentDataEvent, CreateTeleSessionRequestDTO>();

            CreateMap<GetTeleSessionDTO, GetTeleSessionByAppointmentIdContract>();
        }
    }
}
