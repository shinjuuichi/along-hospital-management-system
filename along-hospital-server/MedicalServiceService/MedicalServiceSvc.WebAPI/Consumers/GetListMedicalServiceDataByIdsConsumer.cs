using AutoMapper;
using MassTransit;
using MedicalServiceSvc.BLL.Interfaces;
using MessageBroker.Contracts.MedicalServiceContracts;
using MessageBroker.Events.MedicalServiceEvents;
using SharedLibrary.Base.MessageBuses;

namespace MedicalServiceSvc.WebAPI.Consumers
{
    public class GetListMedicalServiceDataByIdsConsumer(
        IMedicalServiceService medicalServiceService,
        IMapper mapper)
            : RequestConsumer<
                GetListMedicalServiceDataByIdsEvent,
                GetListMedicalServiceDataContract>
    {
        private readonly IMedicalServiceService _medicalServiceService = medicalServiceService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetListMedicalServiceDataContract> Handle(ConsumeContext<GetListMedicalServiceDataByIdsEvent> context)
        {
            var ids = context.Message.Ids;

            var medicalServiceDTOs = await _medicalServiceService.GetAllByIdsAsync(ids);
            var medicalServiceContracts = _mapper.Map<List<GetMedicalServiceContract>>(medicalServiceDTOs);

            return new GetListMedicalServiceDataContract
            {
                Data = medicalServiceContracts
            };
        }
    }
}