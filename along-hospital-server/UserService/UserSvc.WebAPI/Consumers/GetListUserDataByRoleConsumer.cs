using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.UserContracts;
using MessageBroker.Events.UserEvents;
using SharedLibrary.Base.MessageBuses;
using UserSvc.BLL.Interfaces;

namespace UserSvc.WebAPI.Consumers
{
    public class GetListUserDataByRoleConsumer(
        IUserService userService,
        IMapper mapper)
        : RequestConsumer<GetListUserDataByRoleEvent, GetListUserDataByRoleContract>
    {
        private readonly IUserService _userService = userService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetListUserDataByRoleContract> Handle(ConsumeContext<GetListUserDataByRoleEvent> context)
        {
            if (string.IsNullOrEmpty(context.Message.Role))
            {
                throw new InvalidDataException("Role cannot be null or empty");
            }

            var getUserDTOs = await _userService.GetListUserDataByRoleAsync(context.Message.Role);
            var getListUserDataByRoleContract = _mapper.Map<GetListUserDataByRoleContract>(getUserDTOs);
            return getListUserDataByRoleContract;
        }
    }
}