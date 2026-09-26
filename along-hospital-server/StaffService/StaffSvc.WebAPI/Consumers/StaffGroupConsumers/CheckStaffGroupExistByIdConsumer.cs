using MassTransit;
using MessageBroker.Events.StaffEvents.StaffGroupEvents;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Commons.Exceptions;
using StaffSvc.BLL.Interfaces;
using StaffSvc.DAL.Models;

namespace StaffSvc.WebAPI.Consumers.StaffGroupConsumers
{
    public class CheckStaffGroupExistByIdConsumer(IStaffGroupService staffGroupService)
        : RequestConsumer<CheckStaffGroupExistByIdEvent, CheckStaffGroupExistByIdContract>
    {
        private readonly IStaffGroupService _staffGroupService = staffGroupService;

        protected override async Task<CheckStaffGroupExistByIdContract> Handle(ConsumeContext<CheckStaffGroupExistByIdEvent> context)
        {
            var staffGroupId = context.Message.StaffGroupId;

            var isExist = await _staffGroupService.CheckExistByIdAsync(staffGroupId);
            if (!isExist)
            {
                throw new DataNotFoundException(typeof(StaffGroup), staffGroupId);
            }

            return new();
        }
    }
}