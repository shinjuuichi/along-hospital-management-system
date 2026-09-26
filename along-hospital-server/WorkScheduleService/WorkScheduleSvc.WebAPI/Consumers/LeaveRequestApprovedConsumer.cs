using AutoMapper;
using MassTransit;
using MessageBroker.Events.StaffRequestEvents;
using SharedLibrary.Base.MessageBuses;
using WorkScheduleSvc.BLL.DTOs.WorkScheduleDTOs;
using WorkScheduleSvc.BLL.Interfaces;

namespace WorkScheduleSvc.WebAPI.Consumers
{
    public class LeaveRequestApprovedConsumer(
        IWorkScheduleService workScheduleService,
        IMapper mapper)
        : EventConsumer<LeaveRequestApprovedEvent>
    {
        private readonly IWorkScheduleService _workScheduleService = workScheduleService;
        private readonly IMapper _mapper = mapper;

        protected override async Task Handle(ConsumeContext<LeaveRequestApprovedEvent> context)
        {
            var leaveRequestValidationDTO = _mapper.Map<LeaveRequestValidationDTO>(context.Message);
            await _workScheduleService.HandleApprovedLeaveRequestAsync(leaveRequestValidationDTO);
        }
    }
}
