using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Events.AuthAccountEvents.GetUserDataEvents;
using PatientSvc.BLL.Interfaces;
using SharedLibrary.Base.MessageBuses;

namespace PatientSvc.WebAPI.Consumers
{
    public class GetPatientDataByUserIdConsumer(
        IPatientService patientService,
        IMapper mapper)
            : RequestConsumer<GetPatientDataByUserIdEvent, GetPatientDataByUserIdContract>
    {
        private readonly IPatientService _patientService = patientService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetPatientDataByUserIdContract> Handle(ConsumeContext<GetPatientDataByUserIdEvent> context)
        {
            var patientDTO = await _patientService.GetByIdAsync(context.Message.UserId);
            var patientContract = _mapper.Map<GetPatientDataByUserIdContract>(patientDTO);
            return patientContract;
        }
    }
}