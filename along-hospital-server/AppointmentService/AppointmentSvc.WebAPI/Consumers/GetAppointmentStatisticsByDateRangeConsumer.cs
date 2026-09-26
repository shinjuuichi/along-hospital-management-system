using AppointmentSvc.BLL.Interfaces;
using MassTransit;
using MessageBroker.Contracts.AppointmentContracts;
using MessageBroker.Events.AppointmentEvents;
using SharedLibrary.Base.MessageBuses;

namespace AppointmentSvc.WebAPI.Consumers
{
    public class GetAppointmentStatisticsByDateRangeConsumer(IAppointmentStatisticsService appointmentStatisticsService)
        : RequestConsumer<GetAppointmentStatisticsByDateRangeEvent, GetAppointmentStatisticsByDateRangeContract>
    {
        private readonly IAppointmentStatisticsService _appointmentStatisticsService = appointmentStatisticsService;

        protected override async Task<GetAppointmentStatisticsByDateRangeContract> Handle(
            ConsumeContext<GetAppointmentStatisticsByDateRangeEvent> context)
        {
            return await _appointmentStatisticsService.GetStatisticsByDateRangeAsync(
                context.Message.FromDate,
                context.Message.ToDate);
        }
    }
}
