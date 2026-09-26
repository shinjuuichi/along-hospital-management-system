using AuthSvc.BLL.Interfaces;
using MassTransit;
using MessageBroker.Events.StaffEvents;
using SharedLibrary.Base.MessageBuses;

namespace AuthSvc.WebAPI.Consumers
{
    public class ChangeAuthStatusConsumer(IAuthService authService)
        : EventConsumer<ChangeAuthStatusEvent>
    {
        private readonly IAuthService _authService = authService;

        protected override async Task Handle(ConsumeContext<ChangeAuthStatusEvent> context)
        {
            await _authService.ChangeAuthStatusByUserIdAsync(
                context.Message.UserId,
                context.Message.Status);
        }
    }
}