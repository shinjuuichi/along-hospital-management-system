using AutoMapper;
using MassTransit;
using MedicalServiceSvc.BLL.Interfaces;
using MessageBroker.Contracts.MedicalServiceContracts;
using MessageBroker.Events.MedicalServiceEvents;
using SharedLibrary.Base.MessageBuses;

namespace MedicalServiceSvc.WebAPI.Consumers
{
    public class GetListMedicalServiceDataByCodesConsumer(
        IMapper mapper,
        IMedicalServiceService medicalServiceService)
        : RequestConsumer<GetListMedicalServiceDataByCodesEvent, GetListMedicalServiceDataContract>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IMedicalServiceService _medicalServiceService = medicalServiceService;

        protected override async Task<GetListMedicalServiceDataContract> Handle(ConsumeContext<GetListMedicalServiceDataByCodesEvent> context)
        {
            var codes = context.Message.Codes;
            var medicalServicesDTOs = await _medicalServiceService.GetAllByCodesAsync(codes);
            var medicalServiceContracts = _mapper.Map<List<GetMedicalServiceContract>>(medicalServicesDTOs);

            return new GetListMedicalServiceDataContract()
            {
                Data = medicalServiceContracts
            };
        }
    }
}