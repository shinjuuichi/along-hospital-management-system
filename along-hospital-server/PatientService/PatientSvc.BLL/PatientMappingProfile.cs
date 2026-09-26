using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Contracts.PatientContracts;
using MessageBroker.Events.AuthAccountEvents.CreateAccountEvents;
using MessageBroker.Events.AuthAccountEvents.UpdateAccountEvents;
using MessageBroker.Events.PatientEvents;
using PatientSvc.BLL.DTOs;
using SharedLibrary.Base.Mappers;
using System.Reflection;

namespace PatientSvc.BLL
{
    public class PatientMappingProfile : BaseMappingProfile
    {
        public PatientMappingProfile() : base(Assembly.GetExecutingAssembly())
        {
            // DTO to DTO
            CreateMap<CreatePatientAndAccountDTO, CreatePatientDTO>();
            CreateMap<UpdatePatientAndAccountDTO, UpdatePatientDTO>();

            // DTO to Contract
            CreateMap<GetUserDataByUserIdContract, GetPatientDTO>();
            CreateMap<GetPatientDTO, GetPatientDataByUserIdContract>()
                .ForMember(d => d.UserId, opt => opt.MapFrom(s => s.Id));
            CreateMap<GetPatientAllergyDTO, GetPatientAllergyDataContractItem>();
            CreateMap<GetPatientProfileDTO, GetPatientProfileContract>();

            // DTO to Event
            CreateMap<CreatePatientAndAccountDTO, CreateUserToAuthEvent>();
            CreateMap<UpdatePatientAndAccountDTO, UpdateUserToAuthEvent>();
            CreateMap<CreatePatientEvent, CreatePatientDTO>();

            // Contract to Contract
            CreateMap<GetUserDataByUserIdContract, GetPatientDataByUserIdContract>();
        }
    }
}