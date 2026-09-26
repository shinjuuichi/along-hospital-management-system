using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.StaffRequestContracts;
using MessageBroker.Events.StaffRequestEvents;
using SharedLibrary.Base.MessageBuses;
using StaffRequestSvc.BLL.Interfaces;

namespace StaffRequestSvc.WebAPI.Consumers
{
    public class GetUndisbursedSalaryAdvanceByStaffIdConsumer(
        ISalaryAdvanceService salaryAdvanceService,
        IMapper mapper)
        : RequestConsumer<GetUndisbursedSalaryAdvanceByStaffIdEvent, GetUndisbursedSalaryAdvanceByStaffIdContract>
    {
        private readonly ISalaryAdvanceService _salaryAdvanceService = salaryAdvanceService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetUndisbursedSalaryAdvanceByStaffIdContract> Handle(ConsumeContext<GetUndisbursedSalaryAdvanceByStaffIdEvent> context)
        {
            var salaryAdvances = await _salaryAdvanceService.GetUndisbursedByStaffIdAsync(context.Message.StaffId);

            if (salaryAdvances is null)
            {
                return new GetUndisbursedSalaryAdvanceByStaffIdContract();
            }

            var getUndisbursedSalaryAdvanceContract = _mapper.Map<GetUndisbursedSalaryAdvanceByStaffIdContract>(salaryAdvances);
            return getUndisbursedSalaryAdvanceContract;
        }
    }
}
