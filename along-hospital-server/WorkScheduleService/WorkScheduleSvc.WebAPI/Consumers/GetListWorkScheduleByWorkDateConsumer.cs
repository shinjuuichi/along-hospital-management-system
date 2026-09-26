using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.WorkScheduleContracts;
using MessageBroker.Events.WorkScheduleEvents;
using SharedLibrary.Base.MessageBuses;
using WorkScheduleSvc.BLL.Interfaces;

namespace WorkScheduleSvc.WebAPI.Consumers
{
    public class GetListWorkScheduleByWorkDateConsumer(
        IMapper mapper,
        IWorkScheduleService workScheduleService)
        : RequestConsumer<GetListWorkScheduleByWorkDateEvent, GetListWorkScheduleContract>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IWorkScheduleService _workScheduleService = workScheduleService;

        protected override async Task<GetListWorkScheduleContract> Handle(ConsumeContext<GetListWorkScheduleByWorkDateEvent> context)
        {
            var workDate = context.Message.WorkDate;

            var workScheduleDTOs = await _workScheduleService.GetListWorkScheduleByWorkDateAsync(workDate);
            var workScheduleContracts = _mapper.Map<List<GetWorkScheduleContract>>(workScheduleDTOs);

            return new GetListWorkScheduleContract
            {
                Data = workScheduleContracts
            };
        }
    }
}