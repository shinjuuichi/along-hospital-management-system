using MassTransit;
using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Events.AuthAccountEvents.GetUserDataEvents;
using SharedLibrary.Base.MessageBuses;
using UserSvc.BLL.Interfaces;

namespace UserSvc.WebAPI.Consumers
{
    public class GetUserRoleByUserIdConsumer(IUserService _userService)
        : RequestConsumer<GetUserRoleByUserIdEvent, GetUserRoleByUserIdContract>
    {
        protected override async Task<GetUserRoleByUserIdContract> Handle(ConsumeContext<GetUserRoleByUserIdEvent> context)
        {
            var user = await _userService.GetByIdAsync(context.Message.Id);
            return new GetUserRoleByUserIdContract
            {
                Role = user.Role ?? string.Empty
            };
        }
    }
}