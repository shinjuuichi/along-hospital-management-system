using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.PatientContracts;
using MessageBroker.Events.PatientEvents;
using PatientSvc.BLL.Interfaces;
using SharedLibrary.Base.MessageBuses;

namespace PatientSvc.WebAPI.Consumers
{
    public class GetPatientProfileConsumer(
        IPatientService patientService,
        IMapper mapper)
            : RequestConsumer<GetPatientProfileEvent, GetPatientProfileContract>
    {
        private readonly IPatientService _patientService = patientService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetPatientProfileContract> Handle(ConsumeContext<GetPatientProfileEvent> context)
        {
            var patientProfile = await _patientService.GetProfileByPatientIdAsync(context.Message.PatientId);
            var patientProfileContract = _mapper.Map<GetPatientProfileContract>(patientProfile);
            return patientProfileContract;
        }
    }
}