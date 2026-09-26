using MassTransit;
using MedicalHistorySvc.BLL.Commons;
using MedicalHistorySvc.BLL.Interfaces;
using MedicalHistorySvc.DAL.Enums;
using MessageBroker.Events.MedicalHistoryEvents;
using SharedLibrary.Base.MessageBuses;

namespace MedicalHistorySvc.WebAPI.Consumers
{
    public class DraftMedicalHistoryWhenGeneralPaymentCompletedConsumer(
        IMedicalHistoryCommandService medicalHistoryCommandService)
            : EventConsumer<DraftMedicalHistoryWhenGeneralPaymentCompletedEvent>
    {
        private readonly IMedicalHistoryCommandService _medicalHistoryCommandService = medicalHistoryCommandService;

        protected override async Task Handle(ConsumeContext<DraftMedicalHistoryWhenGeneralPaymentCompletedEvent> context)
        {
            var medicalHistoryId = context.Message.Id;
            await _medicalHistoryCommandService.UpdateStatusAsync(medicalHistoryId, MedicalHistoryStatusEnum.Draft, FunctionSourceConstants.FROM_SYSTEM);
        }
    }
}