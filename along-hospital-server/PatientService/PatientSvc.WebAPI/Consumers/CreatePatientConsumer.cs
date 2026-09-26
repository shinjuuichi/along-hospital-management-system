using AutoMapper;
using MassTransit;
using MessageBroker.Events.AuthAccountEvents;
using MessageBroker.Events.PatientEvents;
using PatientSvc.BLL.DTOs;
using PatientSvc.BLL.Interfaces;
using SharedLibrary.Base.MessageBuses;

namespace PatientSvc.WebAPI.Consumers
{
    public class CreatePatientConsumer(
        IPatientService patientService,
        IMapper mapper)
            : RequestConsumer<CreatePatientEvent, CreatePatientContract>
    {
        private readonly IPatientService _patientService = patientService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<CreatePatientContract> Handle(ConsumeContext<CreatePatientEvent> context)
        {
            var createPatientDTO = _mapper.Map<CreatePatientDTO>(context.Message);
            await _patientService.CreateAsync(createPatientDTO);
            return new();
        }
    }
}
