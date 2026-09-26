using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.StaffContracts;
using MessageBroker.Events.StaffEvents.StaffGroupEvents;
using SharedLibrary.Base.MessageBuses;
using StaffSvc.BLL.Interfaces;

namespace StaffSvc.WebAPI.Consumers.StaffGroupConsumers
{
    public class GetListStaffGroupDataByIdsConsumer(
        IStaffGroupService staffGroupService,
        IMapper mapper)
            : RequestConsumer<GetListStaffGroupDataByIdsEvent, GetListStaffGroupDataByIdsContract>
    {
        private readonly IStaffGroupService _staffGroupService = staffGroupService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetListStaffGroupDataByIdsContract> Handle(ConsumeContext<GetListStaffGroupDataByIdsEvent> context)
        {
            var staffGroupIds = context.Message.Ids;

            var staffGroupDTOs = await _staffGroupService.GetAllByIdsAsync(staffGroupIds);
            var result = _mapper.Map<List<GetStaffGroupByIdContract>>(staffGroupDTOs);

            return new()
            {
                Data = result,
            };
        }
    }
}