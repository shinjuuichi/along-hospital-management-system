using InpatientResourceSvc.BLL.Interfaces;
using MassTransit;
using MessageBroker.Events.InPatientResourceEvents;
using SharedLibrary.Base.MessageBuses;

namespace InpatientResourceSvc.WebAPI.Consumers
{
    public class DischargeBedOccupancyByMedicalHistoryIdConsumer(
        IBedOccupancyService bedOccupancyService)
        : RequestConsumer<
            DischargeBedOccupancyByMedicalHistoryIdEvent,
            DischargeBedOccupancyByMedicalHistoryIdContract>
    {
        private readonly IBedOccupancyService _bedOccupancyService = bedOccupancyService;

        protected override async Task<DischargeBedOccupancyByMedicalHistoryIdContract> Handle(
            ConsumeContext<DischargeBedOccupancyByMedicalHistoryIdEvent> context)
        {
            await _bedOccupancyService.DischargeByMedicalHistoryIdAsync(context.Message.MedicalHistoryId);
            return new();
        }
    }
}
