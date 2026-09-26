using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Events.AuthAccountEvents.GetUserDataEvents;
using PatientSvc.BLL.Interfaces;
using SharedLibrary.Base.MessageBuses;

namespace PatientSvc.WebAPI.Consumers
{
    public class GetListPatientDataByUserIdsConsumer(
        IPatientService patientService,
        IMapper mapper,
        IMessageBus messageBus)
        : RequestConsumer<GetListPatientDataByUserIdsEvent, GetListPatientDataByUserIdsContract>
    {
        private readonly IPatientService _patientService = patientService;
        private readonly IMapper _mapper = mapper;
        private readonly IMessageBus _messageBus = messageBus;

        protected override async Task<GetListPatientDataByUserIdsContract> Handle(ConsumeContext<GetListPatientDataByUserIdsEvent> context)
        {
            var userIds = context.Message.UserIds;
            var userContracts = await _messageBus.RequestAsync<GetListUserDataByUserIdsEvent, GetListUserDataByUserIdsContract>(new() { UserIds = userIds });

            var patientDTOs = await _patientService.GetAllByIdsAsync([.. userIds]);
            var patientDictDTOs = patientDTOs.ToDictionary(p => p.Id);

            var patientContracts = _mapper.Map<List<GetPatientDataByUserIdContract>>(userContracts.Data);
            for (int i = 0; i < patientContracts.Count; i++)
            {
                var userId = userContracts.Data.ElementAt(i).UserId;
                if (patientDictDTOs.TryGetValue(userId, out var patientDTO))
                {
                    var allergyDtos = _mapper.Map<List<GetPatientAllergyDataContractItem>>(patientDTO.Allergies);

                    patientContracts[i] = patientContracts[i] with
                    {
                        MedicalNumber = patientDTO.MedicalNumber,
                        Height = patientDTO.Height,
                        Weight = patientDTO.Weight,
                        BloodType = patientDTO.BloodType,
                        Allergies = allergyDtos
                    };
                }
            }

            return new GetListPatientDataByUserIdsContract
            {
                Data = patientContracts
            };
        }
    }
}