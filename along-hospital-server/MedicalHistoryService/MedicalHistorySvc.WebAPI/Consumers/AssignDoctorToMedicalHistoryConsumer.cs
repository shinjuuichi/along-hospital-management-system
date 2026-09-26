using MassTransit;
using MedicalHistorySvc.BLL.Interfaces;
using MessageBroker.Events.MedicalHistoryEvents;
using SharedLibrary.Base.MessageBuses;

namespace MedicalHistorySvc.WebAPI.Consumers
{
    public class AssignDoctorToMedicalHistoryConsumer(IMedicalHistoryCommandService medicalHistoryCommandService)
        : RequestConsumer<AssignDoctorToMedicalHistoryEvent, AssignDoctorToMedicalHistoryContract>
    {
        private readonly IMedicalHistoryCommandService _medicalHistoryCommandService = medicalHistoryCommandService;

        protected override async Task<AssignDoctorToMedicalHistoryContract> Handle(ConsumeContext<AssignDoctorToMedicalHistoryEvent> context)
        {
            var medicalHistoryId = context.Message.MedicalHistoryId;
            var doctorId = context.Message.DoctorId;

            await _medicalHistoryCommandService.AssignDoctorToMedicalHistoryAsync(medicalHistoryId, doctorId);
            return new();
        }
    }
}