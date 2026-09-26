using MassTransit;
using MessageBroker.Contracts.StaffContracts;
using MessageBroker.Events.StaffEvents;
using SharedLibrary.Base.MessageBuses;
using StaffSvc.BLL.Interfaces;

namespace StaffSvc.WebAPI.Consumers.StaffConsumers
{
    public class GetActiveStaffIdsByIdsConsumer(IStaffService staffService)
        : RequestConsumer<GetActiveStaffIdsByIdsEvent, GetActiveStaffIdsByIdsContract>
    {
        private readonly IStaffService _staffService = staffService;

        protected override async Task<GetActiveStaffIdsByIdsContract> Handle(ConsumeContext<GetActiveStaffIdsByIdsEvent> context)
        {
            var staffIds = context.Message.StaffIds;

            var activeStaffIds = await _staffService.GetActiveStaffIdsByIdsAsync(staffIds);
            return new GetActiveStaffIdsByIdsContract
            {
                StaffIds = activeStaffIds
            };
        }
    }
}