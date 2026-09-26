using AutoMapper;
using MassTransit;
using MessageBroker.Events.StaffRequestEvents;
using SharedLibrary.Base.MessageBuses;
using WorkScheduleSvc.BLL.DTOs.WorkScheduleDTOs;
using WorkScheduleSvc.BLL.Interfaces;

namespace WorkScheduleSvc.WebAPI.Consumers;

public class ValidateLeaveRequestConsumer(
    IWorkScheduleService workScheduleService,
    IMapper mapper)
    : RequestConsumer<ValidateLeaveRequestEvent, ValidateLeaveRequestContract>
{
    private readonly IWorkScheduleService _workScheduleService = workScheduleService;
    private readonly IMapper _mapper = mapper;

    protected override async Task<ValidateLeaveRequestContract> Handle(
        ConsumeContext<ValidateLeaveRequestEvent> context)
    {
        var leaveRequestValidationDTO = _mapper.Map<LeaveRequestValidationDTO>(context.Message);
        await _workScheduleService.ValidateLeaveRequestAsync(leaveRequestValidationDTO);

        return new ValidateLeaveRequestContract();
    }
}
