using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.StaffRequestContracts;
using MessageBroker.Events.StaffRequestEvents;
using SharedLibrary.Base.MessageBuses;
using StaffRequestSvc.BLL.DTOs;
using StaffRequestSvc.BLL.Interfaces;

namespace StaffRequestSvc.WebAPI.Consumers
{
    public class GetApprovedLeaveByStaffsAndRangeConsumer(
        ILeaveRequestService leaveRequestService,
        IMapper mapper)
        : RequestConsumer<GetApprovedLeaveByStaffsAndRangeEvent, GetApprovedLeaveByStaffsAndRangeContract>
    {
        private readonly ILeaveRequestService _leaveRequestService = leaveRequestService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetApprovedLeaveByStaffsAndRangeContract> Handle(
            ConsumeContext<GetApprovedLeaveByStaffsAndRangeEvent> context)
        {
            var approvedLeavesByStaffsRangeDTO = _mapper.Map<GetApprovedLeavesByStaffsRangeDTO>(context.Message);

            var approvedLeaveDTOs = await _leaveRequestService.GetApprovedLeavesByStaffsAndRangeAsync(approvedLeavesByStaffsRangeDTO);
            var approvedLeaveContracts = _mapper.Map<List<ApprovedLeaveContract>>(approvedLeaveDTOs);

            return new GetApprovedLeaveByStaffsAndRangeContract
            {
                Data = approvedLeaveContracts
            };
        }
    }
}
