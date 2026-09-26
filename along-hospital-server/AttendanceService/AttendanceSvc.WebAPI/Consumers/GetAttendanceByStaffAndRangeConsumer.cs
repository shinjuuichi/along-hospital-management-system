using AttendanceSvc.BLL.DTOs;
using AttendanceSvc.BLL.Interfaces;
using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.AttendanceContracts;
using MessageBroker.Events.AttendanceEvents;
using SharedLibrary.Base.MessageBuses;

namespace AttendanceSvc.WebAPI.Consumers
{
    public class GetAttendanceByStaffsAndRangeConsumer(
        IAttendanceService attendanceService,
        IMapper mapper)
        : RequestConsumer<GetAttendanceByStaffsAndRangeEvent, GetAttendanceByStaffsAndRangeContract>
    {
        private readonly IAttendanceService _attendanceService = attendanceService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetAttendanceByStaffsAndRangeContract> Handle(ConsumeContext<GetAttendanceByStaffsAndRangeEvent> context)
        {
            var attendanceLogsByStaffsRangeDTO = _mapper.Map<GetAttendanceLogsByStaffsRangeDTO>(context.Message);

            var attendanceLogDTOs = await _attendanceService.GetAttendanceLogsByStaffsAndRangeAsync(attendanceLogsByStaffsRangeDTO);
            var attendanceLogContracts = _mapper.Map<List<AttendanceLogContract>>(attendanceLogDTOs);

            return new GetAttendanceByStaffsAndRangeContract
            {
                Data = attendanceLogContracts
            };
        }
    }
}