using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.UserContracts;
using MessageBroker.Events.UserEvents;
using SharedLibrary.Base.MessageBuses;
using UserSvc.BLL.Interfaces;

namespace UserSvc.WebAPI.Consumers
{
    public class GetListUserDataByListRoleConsumer(
        IUserService userService,
        IMapper mapper)
        : RequestConsumer<GetListUserDataByListRoleEvent, GetListUserDataByListRoleContract>
    {
        private readonly IUserService _userService = userService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetListUserDataByListRoleContract> Handle(ConsumeContext<GetListUserDataByListRoleEvent> context)
        {
            var getUserDTOs = await _userService.GetListUserDataByListRoleAsync(context.Message.Roles);

            var data = _mapper.Map<List<GetUserDataByListRoleContract>>(getUserDTOs);
            return new GetListUserDataByListRoleContract { Data = data };
        }
    }
}