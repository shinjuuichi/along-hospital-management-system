using AutoMapper;
using MassTransit;
using MedicalServiceSvc.BLL.Interfaces;
using MessageBroker.Contracts.MedicalServiceContracts;
using MessageBroker.Events.MedicalServiceEvents;
using SharedLibrary.Base.MessageBuses;

namespace MedicalServiceSvc.WebAPI.Consumers
{
    public class GetMedicalServiceByIdConsumer(
        IMedicalServiceService medicalServiceService,
        IMapper mapper)
        : RequestConsumer<GetMedicalServiceByIdEvent, GetMedicalServiceContract>
    {
        private readonly IMedicalServiceService _medicalServiceService = medicalServiceService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetMedicalServiceContract> Handle(ConsumeContext<GetMedicalServiceByIdEvent> context)
        {
            var medicalServiceDto = await _medicalServiceService.GetByIdAsync(context.Message.Id);
            var medicalServiceContract = _mapper.Map<GetMedicalServiceContract>(medicalServiceDto);
            return medicalServiceContract;
        }
    }
}