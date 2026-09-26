using AuthSvc.BLL.Interfaces;
using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Events.AuthAccountEvents.GetUserDataEvents;
using SharedLibrary.Base.MessageBuses;

namespace AuthSvc.WebAPI.Consumers
{
    public class GetListAuthDataByUserIdsConsumer(IAuthService _authService, IMapper _mapper)
        : RequestConsumer<
            GetListAuthDataByUserIdsEvent,
            GetListAuthDataByUserIdsContract>
    {
        protected override async Task<GetListAuthDataByUserIdsContract> Handle(ConsumeContext<GetListAuthDataByUserIdsEvent> context)
        {
            var authDTOs = await _authService.GetAllByIdsAsync(context.Message.UserIds);
            var authContracts = _mapper.Map<List<GetAuthDataByUserIdContract>>(authDTOs);
            return new GetListAuthDataByUserIdsContract { Data = authContracts };
        }
    }
}
