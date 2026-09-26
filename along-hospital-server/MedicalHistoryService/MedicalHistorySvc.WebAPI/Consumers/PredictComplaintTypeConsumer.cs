using MassTransit;
using MedicalHistorySvc.BLL.Interfaces;
using MessageBroker.Events.MedicalHistoryEvents;
using SharedLibrary.Base.MessageBuses;

namespace MedicalHistorySvc.WebAPI.Consumers
{
    public class PredictComplaintTypeConsumer(IComplaintService complaintService) : EventConsumer<PredictComplaintTypeEvent>
    {
        private readonly IComplaintService _complaintService = complaintService;

        protected override async Task Handle(ConsumeContext<PredictComplaintTypeEvent> context)
        {
            await _complaintService.GetTypePredictionAsync(context.Message.ComplaintId);
        }
    }
}
