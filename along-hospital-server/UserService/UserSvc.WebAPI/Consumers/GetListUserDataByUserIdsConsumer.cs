using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Events.AuthAccountEvents.GetUserDataEvents;
using SharedLibrary.Base.MessageBuses;
using UserSvc.BLL.Interfaces;

namespace UserSvc.WebAPI.Consumers
{
    public class GetListUserDataByUserIdsConsumer(
        IMessageBus messageBus,
        IUserService userService,
        IMapper mapper)
        : RequestConsumer<GetListUserDataByUserIdsEvent, GetListUserDataByUserIdsContract>
    {
        private readonly IMessageBus _messageBus = messageBus;
        private readonly IUserService _userService = userService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetListUserDataByUserIdsContract> Handle(
            ConsumeContext<GetListUserDataByUserIdsEvent> context)
        {
            var getListAuthDataEvent = new GetListAuthDataByUserIdsEvent
            {
                UserIds = context.Message.UserIds
            };

            var authDataResponse = await _messageBus.RequestAsync<GetListAuthDataByUserIdsEvent, GetListAuthDataByUserIdsContract>(getListAuthDataEvent);

            var authDataList = authDataResponse.Data;

            var userIds = context.Message.UserIds;

            var userDTOs = await _userService.GetAllByIdsAsync(userIds);
            var authDataDict = authDataList.ToDictionary(auth => auth.UserId);

            var userDataContracts = _mapper.Map<List<GetUserDataByUserIdContract>>(userDTOs);
            for (int i = 0; i < userDataContracts.Count; i++)
            {
                var userId = userDataContracts[i].UserId;
                if (authDataDict.TryGetValue(userId, out var authData))
                {
                    userDataContracts[i] = userDataContracts[i] with
                    {
                        Phone = authData.Phone,
                        Email = authData.Email
                    };
                }
            }

            return new GetListUserDataByUserIdsContract
            {
                Data = userDataContracts
            };
        }
    }
}