using MassTransit;
using MessageBroker.Events.StaffEvents;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Commons.Exceptions;
using StaffSvc.BLL.Interfaces;
using StaffSvc.DAL.Models;

namespace StaffSvc.WebAPI.Consumers.StaffConsumers
{
    public class CheckStaffExistByIdConsumer(IStaffService staffService)
        : RequestConsumer<CheckStaffExistByIdEvent, CheckStaffExistByIdContract>
    {
        private readonly IStaffService _staffService = staffService;

        protected override async Task<CheckStaffExistByIdContract> Handle(ConsumeContext<CheckStaffExistByIdEvent> context)
        {
            var staffId = context.Message.StaffId;

            var isExist = await _staffService.CheckExistByIdAsync(staffId);
            if (!isExist)
            {
                throw new DataNotFoundException(typeof(Staff), staffId);
            }

            return new();
        }
    }
}