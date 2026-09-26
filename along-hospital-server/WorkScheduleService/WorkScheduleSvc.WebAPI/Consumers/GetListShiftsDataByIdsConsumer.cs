using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.WorkScheduleContracts;
using MessageBroker.Events.WorkScheduleEvents;
using SharedLibrary.Base.MessageBuses;
using WorkScheduleSvc.BLL.Interfaces;

namespace WorkScheduleSvc.WebAPI.Consumers
{
    public class GetListShiftsDataByIdsConsumer(
        IMapper mapper,
        IShiftService shiftService)
        : RequestConsumer<GetListShiftsDataByIdsEvent, GetListShiftsDataByIdsContract>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IShiftService _shiftService = shiftService;

        protected override async Task<GetListShiftsDataByIdsContract> Handle(ConsumeContext<GetListShiftsDataByIdsEvent> context)
        {
            var shiftDTOs = await _shiftService.GetAllByIdsAsync(context.Message.Ids);
            var shiftContracts = _mapper.Map<List<GetShiftContract>>(shiftDTOs);

            return new GetListShiftsDataByIdsContract
            {
                Data = shiftContracts
            };
        }
    }
}
