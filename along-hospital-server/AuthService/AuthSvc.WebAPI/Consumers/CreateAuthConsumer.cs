using AuthSvc.BLL.DTOs;
using AuthSvc.BLL.Interfaces;
using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.AuthAccountContracts.CreateAccountContracts;
using MessageBroker.Events.AuthAccountEvents.CreateAccountEvents;
using SharedLibrary.Base.MessageBuses;

namespace AuthSvc.WebAPI.Consumers
{
    public class CreateAuthConsumer(IAuthService _authService, IMapper _mapper) : RequestConsumer<CreateAuthEvent, CreateAuthContract>
    {
        protected override async Task<CreateAuthContract> Handle(ConsumeContext<CreateAuthEvent> context)
        {
            var createAuth = _mapper.Map<CreateAuthDTO>(context.Message);
            await _authService.CreateAsync(createAuth);
            return new();
        }
    }
}
