using AutoMapper;
using MassTransit;
using MedicalServiceSvc.BLL.Interfaces;
using MessageBroker.Contracts.MedicalServiceContracts;
using MessageBroker.Events.MedicalServiceEvents;
using SharedLibrary.Base.MessageBuses;

namespace MedicalServiceSvc.WebAPI.Consumers
{
    public class GetMedicalServiceByCodeConsumer(
        IMedicalServiceService medicalServiceService,
        IMapper mapper)
        : RequestConsumer<GetMedicalServiceByCodeEvent, GetMedicalServiceContract>
    {
        private readonly IMedicalServiceService _medicalServiceService = medicalServiceService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetMedicalServiceContract> Handle(ConsumeContext<GetMedicalServiceByCodeEvent> context)
        {
            var code = context.Message.Code;
            if (string.IsNullOrEmpty(code))
            {
                throw new InvalidDataException("Medical service code is required");
            }

            var getMedicalServiceDto = await _medicalServiceService.GetByCodeAsync(code);
            return _mapper.Map<GetMedicalServiceContract>(getMedicalServiceDto);
        }
    }
}
