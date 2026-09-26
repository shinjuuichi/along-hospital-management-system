using MassTransit;
using MessageBroker.Events.AuthAccountEvents;
using MessageBroker.Events.FeedbackEvents;
using SharedLibrary.Base.MessageBuses;
using UserSvc.BLL.Interfaces;

namespace UserSvc.WebAPI.Consumers
{
    public class DeleteUserByIdWhenCrashingInUserConsumer(IUserService userService, IMessageBus messageBus)
        : EventConsumer<DeleteUserByIdWhenCrashingEvent>
    {
        private readonly IUserService _userService = userService;
        private readonly IMessageBus _messageBus = messageBus;

        protected override async Task Handle(ConsumeContext<DeleteUserByIdWhenCrashingEvent> context)
        {
            await _userService.DeleteAsync(context.Message.Id);

            await _messageBus.PublishAsync(new BanFeedbackEvent
            {
                UserId = context.Message.Id
            });
        }
    }
}