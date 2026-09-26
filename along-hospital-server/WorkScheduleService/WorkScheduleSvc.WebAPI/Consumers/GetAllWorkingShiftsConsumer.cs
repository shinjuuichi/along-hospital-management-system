using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.WorkScheduleContracts;
using MessageBroker.Events.WorkScheduleEvents;
using SharedLibrary.Base.MessageBuses;
using WorkScheduleSvc.BLL.Interfaces;

namespace WorkScheduleSvc.WebAPI.Consumers
{
    public class GetAllWorkingShiftsConsumer(
        IMapper mapper,
        IShiftService shiftService)
        : RequestConsumer<GetAllWorkingShiftsEvent, GetAllWorkingShiftsContract>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IShiftService _shiftService = shiftService;

        protected override async Task<GetAllWorkingShiftsContract> Handle(ConsumeContext<GetAllWorkingShiftsEvent> context)
        {
            var shiftDTOs = await _shiftService.GetAllAsync();
            var workingShiftDTOs = shiftDTOs
                .Where(shift => !shift.IsOvertime)
                .OrderBy(shift => shift.StartTime)
                .ToList();

            var shiftContracts = _mapper.Map<List<GetShiftContract>>(workingShiftDTOs);

            return new GetAllWorkingShiftsContract
            {
                Data = shiftContracts
            };
        }
    }
}