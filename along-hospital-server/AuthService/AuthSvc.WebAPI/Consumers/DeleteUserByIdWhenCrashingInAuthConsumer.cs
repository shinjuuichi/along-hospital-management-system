using AuthSvc.BLL.Interfaces;
using MassTransit;
using MessageBroker.Events.AuthAccountEvents;
using SharedLibrary.Base.MessageBuses;

namespace AuthSvc.WebAPI.Consumers
{
    public class DeleteUserByIdWhenCrashingInAuthConsumer(IAuthService _authService)
        : EventConsumer<DeleteUserByIdWhenCrashingEvent>
    {
        protected override async Task Handle(ConsumeContext<DeleteUserByIdWhenCrashingEvent> context)
        {
            await _authService.DeleteAsync(context.Message.Id);
        }
    }
}