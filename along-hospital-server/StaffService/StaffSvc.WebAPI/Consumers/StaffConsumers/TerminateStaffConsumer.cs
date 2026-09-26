using MassTransit;
using MessageBroker.Events.StaffEvents;
using SharedLibrary.Base.MessageBuses;
using StaffSvc.BLL.Interfaces;
using StaffSvc.DAL.Enums;

namespace StaffSvc.WebAPI.Consumers.StaffConsumers
{
    public class TerminateStaffConsumer(IStaffService staffService)
        : EventConsumer<TerminateStaffEvent>
    {
        private readonly IStaffService _staffService = staffService;

        protected override async Task Handle(ConsumeContext<TerminateStaffEvent> context)
        {
            await _staffService.UpdateStatusAsync(context.Message.StaffIds, StaffStatusEnum.Terminated);
        }
    }
}
