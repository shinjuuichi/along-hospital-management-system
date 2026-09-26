using MassTransit;
using MedicalOrderSvc.BLL.Interfaces;
using MessageBroker.Events.MedicalOrderEvents;
using SharedLibrary.Base.MessageBuses;

namespace MedicalOrderSvc.WebAPI.Consumers
{
    public class CancelPendingOrDraftMedicalOrdersByMedicalHistoryIdConsumer(
        IMedicalOrderService medicalOrderService)
            : RequestConsumer<CancelPendingOrDraftMedicalOrdersByMedicalHistoryIdEvent, CancelPendingOrDraftMedicalOrdersByMedicalHistoryIdContract>
    {
        private readonly IMedicalOrderService _medicalOrderService = medicalOrderService;

        protected override async Task<CancelPendingOrDraftMedicalOrdersByMedicalHistoryIdContract> Handle(ConsumeContext<CancelPendingOrDraftMedicalOrdersByMedicalHistoryIdEvent> context)
        {
            await _medicalOrderService.CancelPendingOrDraftByMedicalHistoryIdAsync(context.Message.MedicalHistoryId);
            return new CancelPendingOrDraftMedicalOrdersByMedicalHistoryIdContract();
        }
    }
}