using AutoMapper;
using MassTransit;
using MedicalServiceSvc.BLL.Interfaces;
using MessageBroker.Contracts.MedicalServiceContracts;
using MessageBroker.Events.MedicalServiceEvents;
using SharedLibrary.Base.MessageBuses;

namespace MedicalServiceSvc.WebAPI.Consumers
{
    public class GetAllMedicalServicesConsumer(IMedicalServiceService medicalServiceService, IMapper mapper)
        : RequestConsumer<GetAllMedicalServicesEvent, GetAllMedicalServicesContract>
    {
        private readonly IMedicalServiceService _medicalServiceService = medicalServiceService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetAllMedicalServicesContract> Handle(ConsumeContext<GetAllMedicalServicesEvent> context)
        {
            var medicalServices = await _medicalServiceService.GetAllAsync();
            var medicalServicesContractItems = _mapper.Map<List<GetAllMedicalServicesContractItem>>(medicalServices);
            return new GetAllMedicalServicesContract
            {
                MedicalServices = medicalServicesContractItems
            };
        }
    }
}
