using MassTransit;
using MessageBroker.Contracts.PatientContracts;
using MessageBroker.Events.PatientEvents;
using PatientSvc.BLL.Interfaces;
using PatientSvc.DAL.Models;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Commons.Exceptions;

namespace PatientSvc.WebAPI.Consumers
{
    public class CheckPatientExistByIdConsumer(
        IPatientService patientService)
            : RequestConsumer<CheckPatientExistByIdEvent, CheckPatientExistByIdContract>
    {
        private readonly IPatientService _patientService = patientService;

        protected override async Task<CheckPatientExistByIdContract> Handle(ConsumeContext<CheckPatientExistByIdEvent> context)
        {
            var patientId = context.Message.PatientId;
            var isExist = await _patientService.CheckExistByIdAsync(patientId);
            if (!isExist)
            {
                throw new DataNotFoundException(typeof(Patient), patientId);
            }

            return new CheckPatientExistByIdContract
            {
                IsSuccess = true
            };
        }
    }
}