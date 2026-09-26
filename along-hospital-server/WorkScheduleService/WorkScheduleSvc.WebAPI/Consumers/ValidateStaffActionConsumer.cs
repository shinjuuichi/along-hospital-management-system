using MassTransit;
using MessageBroker.Events.WorkScheduleEvents;
using SharedLibrary.Base.MessageBuses;
using WorkScheduleSvc.BLL.Interfaces.Querys;

namespace WorkScheduleSvc.WebAPI.Consumers
{
    public class ValidateStaffActionConsumer(
        IWorkScheduleQueryService workScheduleQueryService)
        : RequestConsumer<ValidateStaffActionEvent, ValidateStaffActionContract>
    {
        private readonly IWorkScheduleQueryService _workScheduleQueryService = workScheduleQueryService;

        protected override async Task<ValidateStaffActionContract> Handle(ConsumeContext<ValidateStaffActionEvent> context)
        {
            var message = context.Message;
            await _workScheduleQueryService.ValidateStaffActionByRoomIdAsync(message.StaffId, message.RoomId);

            return new ValidateStaffActionContract();
        }
    }
}
