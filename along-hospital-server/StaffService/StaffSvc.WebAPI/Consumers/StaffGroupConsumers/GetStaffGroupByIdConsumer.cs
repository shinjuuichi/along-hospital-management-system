using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.StaffContracts;
using MessageBroker.Events.StaffEvents.StaffGroupEvents;
using SharedLibrary.Base.MessageBuses;
using StaffSvc.BLL.Interfaces;

namespace StaffSvc.WebAPI.Consumers.StaffGroupConsumers
{
    public class GetStaffGroupByIdConsumer(
        IStaffGroupService staffGroupService,
        IMapper mapper)
          : RequestConsumer<GetStaffGroupByIdEvent, GetStaffGroupByIdContract>
    {
        private readonly IStaffGroupService _staffGroupService = staffGroupService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetStaffGroupByIdContract> Handle(ConsumeContext<GetStaffGroupByIdEvent> context)
        {
            var staffGroupId = context.Message.Id;

            var staffGroupDTO = await _staffGroupService.GetByIdAsync(staffGroupId);
            var result = _mapper.Map<GetStaffGroupByIdContract>(staffGroupDTO);

            return result;
        }
    }
}