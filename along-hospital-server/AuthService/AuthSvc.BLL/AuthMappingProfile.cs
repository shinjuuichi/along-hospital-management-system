using System.Reflection;
using AuthSvc.BLL.DTOs;
using AuthSvc.BLL.DTOs.Request;
using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Events.AuthAccountEvents;
using MessageBroker.Events.AuthAccountEvents.CreateAccountEvents;
using MessageBroker.Events.AuthAccountEvents.UpdateAccountEvents;
using SharedLibrary.Base.Mappers;

namespace AuthSvc.BLL
{
    public class AuthMappingProfile : BaseMappingProfile
    {
        public AuthMappingProfile() : base(Assembly.GetExecutingAssembly())
        {
            CreateMap<CreateAuthEvent, CreateAuthDTO>();
            CreateMap<UpdateAuthEvent, UpdateAuthDTO>();
            CreateMap<UpdateAuthAccountWithUserIdEvent, UpdateAuthAccountWithUserIdDTO>();

            CreateMap<GetAuthDTO, GetAuthDataByUserIdContract>();

            CreateMap<GetListUserIdByFilterAuthAccountEntityEvent, AuthFilterRequestDTO>();
        }
    }
}
