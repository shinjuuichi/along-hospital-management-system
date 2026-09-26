using MassTransit;
using MessageBroker.Events.AppointmentEvents;
using QueueSvc.BLL.Interfaces;
using SharedLibrary.Base.MessageBuses;

namespace QueueSvc.WebAPI.Consumers
{
    public class SyncAppointmentToQueueCacheConsumer(
        IQueueCacheService queueCacheService)
        : EventConsumer<SyncAppointmentToQueueCacheEvent>
    {
        private readonly IQueueCacheService _queueCacheService = queueCacheService;

        protected override async Task Handle(ConsumeContext<SyncAppointmentToQueueCacheEvent> context)
        {
            var appointmentContract = context.Message.Appointment;
            if (appointmentContract == null)
            {
                return;
            }

            await _queueCacheService.UpdateAppointmentInCacheAsync(appointmentContract.Date, appointmentContract.Id, appointmentContract);
        }
    }
}
