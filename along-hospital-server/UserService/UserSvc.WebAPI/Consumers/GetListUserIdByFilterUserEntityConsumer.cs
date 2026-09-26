using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.UserContracts;
using MessageBroker.Events.UserEvents;
using SharedLibrary.Base.MessageBuses;
using UserSvc.BLL.DTOs;
using UserSvc.BLL.Interfaces;

namespace UserSvc.WebAPI.Consumers
{
    public class GetListUserIdByFilterUserEntityConsumer(
        IUserService userService,
        IMapper mapper)
        : RequestConsumer<GetListUserIdByFilterUserEntityEvent, GetListUserIdByFilterUserEntityContract>
    {
        private readonly IUserService _userService = userService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetListUserIdByFilterUserEntityContract> Handle(ConsumeContext<GetListUserIdByFilterUserEntityEvent> context)
        {
            var userFilterRequestDTO = _mapper.Map<UserFilterRequestDTO>(context.Message);

            var userIds = await _userService.GetUserIdsByFilterAsync(userFilterRequestDTO);

            return new GetListUserIdByFilterUserEntityContract
            {
                UserIds = userIds
            };
        }
    }
}