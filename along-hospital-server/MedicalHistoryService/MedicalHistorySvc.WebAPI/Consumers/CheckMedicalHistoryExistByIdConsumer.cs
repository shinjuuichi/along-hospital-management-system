using MassTransit;
using MedicalHistorySvc.BLL.Interfaces;
using MedicalHistorySvc.DAL.Models;
using MessageBroker.Events.MedicalHistoryEvents;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Commons.Exceptions;

namespace MedicalHistorySvc.WebAPI.Consumers
{
    public class CheckMedicalHistoryExistByIdConsumer(IMedicalHistoryQueryService medicalHistoryService)
        : RequestConsumer<CheckMedicalHistoryExistByIdEvent, CheckMedicalHistoryExistByIdContract>
    {
        private readonly IMedicalHistoryQueryService _medicalHistoryService = medicalHistoryService;

        protected override async Task<CheckMedicalHistoryExistByIdContract> Handle(ConsumeContext<CheckMedicalHistoryExistByIdEvent> context)
        {
            var medicalHistoryId = context.Message.Id;

            var isExist = await _medicalHistoryService.CheckExistByIdAsync(medicalHistoryId);
            if (!isExist)
            {
                throw new DataNotFoundException(typeof(MedicalHistory), medicalHistoryId);
            }

            return new();
        }
    }
}