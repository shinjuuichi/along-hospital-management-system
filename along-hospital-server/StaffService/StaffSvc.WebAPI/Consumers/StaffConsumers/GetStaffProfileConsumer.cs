using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.StaffContracts;
using MessageBroker.Events.StaffEvents;
using SharedLibrary.Base.MessageBuses;
using StaffSvc.BLL.Interfaces;

namespace StaffSvc.WebAPI.Consumers.StaffConsumers
{
    public class GetStaffProfileConsumer(
        IStaffService staffService,
        IMapper mapper)
        : RequestConsumer<GetStaffProfileEvent, GetStaffProfileContract>
    {
        private readonly IStaffService _staffService = staffService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetStaffProfileContract> Handle(ConsumeContext<GetStaffProfileEvent> context)
        {
            var staffProfile = await _staffService.GetByIdAsync(context.Message.StaffId);
            var staffProfileContract = _mapper.Map<GetStaffProfileContract>(staffProfile);

            return staffProfileContract;
        }
    }
}