using AuthSvc.BLL.DTOs;
using AuthSvc.BLL.Interfaces;
using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.AuthAccountContracts;
using MessageBroker.Events.AuthAccountEvents;
using SharedLibrary.Base.MessageBuses;

namespace AuthSvc.WebAPI.Consumers
{
    public class GetListUserIdByFilterAuthAccountEntityConsumer(
        IAuthService authService,
        IMapper mapper)
        : RequestConsumer<GetListUserIdByFilterAuthAccountEntityEvent, GetListUserIdByFilterAuthAccountEntityContract>
    {
        private readonly IAuthService _authService = authService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetListUserIdByFilterAuthAccountEntityContract> Handle(ConsumeContext<GetListUserIdByFilterAuthAccountEntityEvent> context)
        {
            var authFilterRequestDto = _mapper.Map<AuthFilterRequestDTO>(context.Message);

            var userIds = await _authService.GetUserIdsByFilterAsync(authFilterRequestDto);

            return new GetListUserIdByFilterAuthAccountEntityContract
            {
                UserIds = userIds
            };
        }
    }
}