using MassTransit;
using MessageBroker.Events.StaffEvents;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Commons.Exceptions;
using StaffSvc.BLL.Interfaces;

namespace StaffSvc.WebAPI.Consumers.StaffConsumers
{
    public class CheckStaffExistByIdsConsumer(IStaffService staffService)
        : RequestConsumer<CheckStaffExistByIdsEvent, CheckStaffExistByIdsContract>
    {
        private readonly IStaffService _staffService = staffService;

        protected override async Task<CheckStaffExistByIdsContract> Handle(ConsumeContext<CheckStaffExistByIdsEvent> context)
        {
            var staffIds = context.Message.StaffIds;

            var isExist = await _staffService.CheckExistByIdsAsync(staffIds);
            if (!isExist)
            {
                throw new DataNotFoundException("One or more staff Ids do not exist.");
            }

            return new();
        }
    }
}