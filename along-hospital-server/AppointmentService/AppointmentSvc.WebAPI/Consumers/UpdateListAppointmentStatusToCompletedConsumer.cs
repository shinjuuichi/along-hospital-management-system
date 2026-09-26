using AppointmentSvc.BLL.Interfaces;
using AppointmentSvc.DAL.Enums;
using MassTransit;
using MessageBroker.Events.AppointmentEvents;
using SharedLibrary.Base.MessageBuses;

namespace AppointmentSvc.WebAPI.Consumers
{
    public class UpdateListAppointmentStatusToCompletedConsumer(IAppointmentCommandService appointmentCommandService)
        : EventConsumer<UpdateListAppointmentStatusToCompletedEvent>
    {
        private readonly IAppointmentCommandService _appointmentCommandService = appointmentCommandService;

        protected override async Task Handle(ConsumeContext<UpdateListAppointmentStatusToCompletedEvent> context)
        {
            var appointmentIds = context.Message.Ids;

            await _appointmentCommandService.UpdateListStatusAsync(appointmentIds, AppointmentStatusEnum.Completed);
        }
    }
}
