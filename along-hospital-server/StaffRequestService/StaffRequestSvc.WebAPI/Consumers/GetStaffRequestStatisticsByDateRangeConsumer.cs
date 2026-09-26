using MassTransit;
using MessageBroker.Contracts.StaffRequestContracts;
using MessageBroker.Events.StaffRequestEvents;
using SharedLibrary.Base.MessageBuses;
using StaffRequestSvc.BLL.Interfaces;

namespace StaffRequestSvc.WebAPI.Consumers;

public class GetStaffRequestStatisticsByDateRangeConsumer(IStaffRequestStatisticsService staffRequestStatisticsService)
    : RequestConsumer<GetStaffRequestStatisticsByDateRangeEvent, GetStaffRequestStatisticsByDateRangeContract>
{
    private readonly IStaffRequestStatisticsService _staffRequestStatisticsService = staffRequestStatisticsService;

    protected override async Task<GetStaffRequestStatisticsByDateRangeContract> Handle(
        ConsumeContext<GetStaffRequestStatisticsByDateRangeEvent> context)
    {
        return await _staffRequestStatisticsService.GetStatisticsByDateRangeAsync(
            context.Message.FromDate,
            context.Message.ToDate);
    }
}
