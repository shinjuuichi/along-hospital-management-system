using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Events.AuthAccountEvents.GetUserDataEvents;
using SharedLibrary.Base.MessageBuses;
using UserSvc.BLL.Interfaces;

namespace UserSvc.WebAPI.Consumers
{
    public class GetUserDataByUserIdConsumer(
        IMessageBus messageBus,
        IUserService userService,
        IMapper mapper)
        : RequestConsumer<GetUserDataByUserIdEvent, GetUserDataByUserIdContract>
    {
        private readonly IMessageBus _messageBus = messageBus;
        private readonly IUserService _userService = userService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetUserDataByUserIdContract> Handle(ConsumeContext<GetUserDataByUserIdEvent> context)
        {
            var getAuthDataEvent = new GetAuthDataByUserIdEvent
            {
                UserId = context.Message.UserId
            };

            var authData = await _messageBus.RequestAsync<GetAuthDataByUserIdEvent, GetAuthDataByUserIdContract>(getAuthDataEvent);

            var userDto = await _userService.GetByIdAsync(context.Message.UserId);

            var userContract = _mapper.Map<GetUserDataByUserIdContract>(userDto);

            return userContract with
            {
                Phone = authData.Phone,
                Email = authData.Email
            };
        }
    }
}