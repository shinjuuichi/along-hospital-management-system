using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.StaffContracts;
using MessageBroker.Events.StaffEvents;
using SharedLibrary.Base.MessageBuses;
using StaffSvc.BLL.Interfaces;

namespace StaffSvc.WebAPI.Consumers.StaffConsumers
{
    public class GetStaffContractByStaffIdConsumer(
        IStaffContractService staffContractService,
        IStaffService staffService,
        IMapper mapper)
        : RequestConsumer<GetStaffContractByStaffIdEvent, GetStaffContractByStaffIdContract>
    {
        private readonly IStaffContractService _staffContractService = staffContractService;
        private readonly IStaffService _staffService = staffService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetStaffContractByStaffIdContract> Handle(ConsumeContext<GetStaffContractByStaffIdEvent> context)
        {
            var staffId = context.Message.StaffId;

            var staffContract = await _staffContractService.GetActiveContractByStaffIdAsync(staffId);
            var staff = await _staffService.GetByIdAsync(staffId);

            var contract = _mapper.Map<GetStaffContractByStaffIdContract>(staffContract);
            _mapper.Map(staff, contract);

            return contract;
        }
    }
}
