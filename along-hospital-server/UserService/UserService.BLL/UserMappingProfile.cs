using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Contracts.PatientContracts;
using MessageBroker.Contracts.StaffContracts;
using MessageBroker.Contracts.UserContracts;
using MessageBroker.Events.AuthAccountEvents.CreateAccountEvents;
using MessageBroker.Events.AuthAccountEvents.UpdateAccountEvents;
using MessageBroker.Events.UserEvents;
using SharedLibrary.Base.Mappers;
using System.Reflection;
using UserSvc.BLL.DTOs;
using UserSvc.BLL.DTOs.StatisticsDTOs;

namespace UserSvc.BLL
{
    public class UserMappingProfile : BaseMappingProfile
    {
        public UserMappingProfile()
            : base(Assembly.GetExecutingAssembly())
        {

            // Create And Update Mappings (DTO, Event)
            CreateMap<UpdateUserToAuthEvent, UpdateUserDTO>();
            CreateMap<UpdateUserToAuthEvent, UpdateAuthEvent>();
            CreateMap<UpdateUserEvent, UpdateUserDTO>();
            CreateMap<CreateUserWithRolePatientProfileDTO, CreateUserDTO>();
            CreateMap<CreateUserToAuthEvent, CreateUserDTO>();
            CreateMap<CreateUserToAuthEvent, CreateAuthEvent>();

            // Profile Mappings
            CreateMap<GetAuthDataByUserIdContract, UserProfileDTO>();
            CreateMap<UserProfileDTO, PatientProfileDTO>();
            CreateMap<UserProfileDTO, DoctorProfileDTO>();
            CreateMap<GetPatientProfileContract, PatientProfileDTO>();
            CreateMap<GetStaffProfileContract, DoctorProfileDTO>();

            // DTO, Contract Mappings
            CreateMap<UserGrowthStatisticsDTO, GetUserGrowthStatisticsByDateRangeContract>();
            CreateMap<UpdateUserDTO, UpdateUserContract>();
            CreateMap<GetUserDTO, GetUserDataByUserIdContract>()
                .ForMember(d => d.UserId, o => o.MapFrom(s => s.Id));

            CreateMap<GetListUserIdByFilterUserEntityEvent, UserFilterRequestDTO>();
            CreateMap<UserProfileDTO, GetUserDataByRoleContract>();
            CreateMap<UserEmailDTO, UserEmailData>();
            CreateMap<GetListUserDataByRoleContract, GetUserDTO>();
            CreateMap<GetUserDTO, GetUserDataByListRoleContract>()
                .ForMember(d => d.UserId, o => o.MapFrom(s => s.Id));
        }
    }
}
