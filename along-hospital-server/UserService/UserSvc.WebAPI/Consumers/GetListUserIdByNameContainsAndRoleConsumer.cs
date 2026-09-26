using MassTransit;
using MessageBroker.Contracts.UserContracts;
using MessageBroker.Events.UserEvents;
using SharedLibrary.Base.MessageBuses;
using UserSvc.BLL.Interfaces;

namespace UserSvc.WebAPI.Consumers
{
    public class GetListUserIdByNameContainsAndRoleConsumer(IUserService userService)
        : RequestConsumer<GetListUserIdByNameContainsAndRoleEvent, GetListUserIdByNameContainsAndRoleContract>
    {
        private readonly IUserService _userService = userService;

        protected override async Task<GetListUserIdByNameContainsAndRoleContract> Handle(
            ConsumeContext<GetListUserIdByNameContainsAndRoleEvent> context)
        {
            var nameRoleRequest = context.Message;
            var userIds = await _userService.GetListUserIdByNameContainsAndRoleAsync(
                nameRoleRequest.Name,
                nameRoleRequest.Role);

            return new GetListUserIdByNameContainsAndRoleContract
            {
                IsSuccess = true,
                Ids = userIds
            };
        }
    }
}