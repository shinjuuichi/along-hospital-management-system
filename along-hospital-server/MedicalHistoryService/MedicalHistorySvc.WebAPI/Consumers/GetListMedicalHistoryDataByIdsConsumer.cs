using AutoMapper;
using MassTransit;
using MedicalHistorySvc.BLL.Interfaces;
using MessageBroker.Contracts.MedicalHistoryContracts;
using MessageBroker.Events.MedicalHistoryEvents;
using SharedLibrary.Base.MessageBuses;

namespace MedicalHistorySvc.WebAPI.Consumers
{
    public class GetListMedicalHistoryDataByIdsConsumer(
        IMedicalHistoryQueryService medicalHistoryQueryService,
        IMapper mapper)
            : RequestConsumer<
                GetListMedicalHistoryDataByIdsEvent,
                GetListMedicalHistoryDataContract>
    {
        private readonly IMedicalHistoryQueryService _medicalHistoryQueryService = medicalHistoryQueryService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetListMedicalHistoryDataContract> Handle(ConsumeContext<GetListMedicalHistoryDataByIdsEvent> context)
        {
            var medicalHistoryDTOs = await _medicalHistoryQueryService.GetAllByIdsAsync(context.Message.Ids);
            var medicalHistoryContract = _mapper.Map<List<GetMedicalHistoryContract>>(medicalHistoryDTOs);
            return new() { Data = medicalHistoryContract };
        }
    }
}
