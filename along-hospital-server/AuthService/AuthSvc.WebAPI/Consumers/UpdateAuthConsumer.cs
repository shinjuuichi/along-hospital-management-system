using AuthSvc.BLL.DTOs;
using AuthSvc.BLL.Interfaces;
using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.AuthAccountContracts.UpdateAccountContracts;
using MessageBroker.Events.AuthAccountEvents.UpdateAccountEvents;
using SharedLibrary.Base.MessageBuses;

namespace AuthSvc.WebAPI.Consumers
{
    public class UpdateAuthConsumer(IAuthService _authService, IMapper _mapper) : RequestConsumer<UpdateAuthEvent, UpdateAuthContract>
    {
        protected override async Task<UpdateAuthContract> Handle(ConsumeContext<UpdateAuthEvent> context)
        {
            var updateAuth = _mapper.Map<UpdateAuthDTO>(context.Message);
            await _authService.UpdateAsync(context.Message.UserId, updateAuth);
            return new();
        }
    }
}