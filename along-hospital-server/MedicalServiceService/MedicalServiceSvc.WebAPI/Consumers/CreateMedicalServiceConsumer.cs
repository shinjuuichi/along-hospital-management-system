using AutoMapper;
using MassTransit;
using MedicalServiceSvc.BLL.DTOs;
using MedicalServiceSvc.BLL.Interfaces;
using MessageBroker.Contracts.MedicalServiceContracts;
using MessageBroker.Events.MedicalServiceEvents;
using SharedLibrary.Base.MessageBuses;

namespace MedicalServiceSvc.WebAPI.Consumers
{
    public class CreateMedicalServiceConsumer(
        IMapper mapper,
        IMedicalServiceService medicalServiceService)
        : RequestConsumer<CreateMedicalServiceEvent, CreateMedicalServiceContract>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IMedicalServiceService _medicalServiceService = medicalServiceService;

        protected override async Task<CreateMedicalServiceContract> Handle(ConsumeContext<CreateMedicalServiceEvent> context)
        {
            var createDTO = _mapper.Map<UpsertMedicalServiceDTO>(context.Message);
            var result = await _medicalServiceService.CreateAsync(createDTO);

            return _mapper.Map<CreateMedicalServiceContract>(result);
        }
    }
}