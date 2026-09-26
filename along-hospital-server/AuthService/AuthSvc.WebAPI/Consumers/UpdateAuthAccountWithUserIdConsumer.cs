using AuthSvc.BLL.DTOs.Request;
using AuthSvc.BLL.Interfaces;
using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.AuthAccountContracts;
using MessageBroker.Events.AuthAccountEvents;
using SharedLibrary.Base.MessageBuses;

namespace AuthSvc.WebAPI.Consumers
{
    public class UpdateAuthAccountWithUserIdConsumer(IAuthService _authService, IMapper _mapper) : RequestConsumer<UpdateAuthAccountWithUserIdEvent, UpdateAuthAccountWithUserIdContact>
    {
        protected override async Task<UpdateAuthAccountWithUserIdContact> Handle(ConsumeContext<UpdateAuthAccountWithUserIdEvent> context)
        {
            var dto = _mapper.Map<UpdateAuthAccountWithUserIdDTO>(context.Message);
            await _authService.UpdateAuthAccountWithUserIdAsync(dto);

            return new UpdateAuthAccountWithUserIdContact
            {
                IsSuccess = true
            };
        }
    }
}
