using AuthSvc.BLL.Interfaces;
using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Events.AuthAccountEvents.GetUserDataEvents;
using SharedLibrary.Base.MessageBuses;

namespace AuthSvc.WebAPI.Consumers
{
    public class GetAuthDataByUserIdConsumer(IAuthService _authService, IMapper _mapper)
        : RequestConsumer<
            GetAuthDataByUserIdEvent,
            GetAuthDataByUserIdContract>
    {
        protected override async Task<GetAuthDataByUserIdContract> Handle(ConsumeContext<GetAuthDataByUserIdEvent> context)
        {
            var authDto = await _authService.GetByIdAsync(context.Message.UserId);
            var authContract = _mapper.Map<GetAuthDataByUserIdContract>(authDto);
            return authContract;
        }
    }
}
