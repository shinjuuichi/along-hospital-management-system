using AutoMapper;
using MassTransit;
using MedicalHistorySvc.BLL.Interfaces;
using MessageBroker.Contracts.MedicalHistoryContracts;
using MessageBroker.Events.MedicalHistoryEvents;
using SharedLibrary.Base.MessageBuses;

namespace MedicalHistorySvc.WebAPI.Consumers
{
    public class GetListMedicalHistoryOccupancySummaryByIdsConsumer(
        IMedicalHistoryQueryService medicalHistoryQueryService,
        IMapper mapper)
            : RequestConsumer<
                GetListMedicalHistoryOccupancySummaryByIdsEvent,
                GetListMedicalHistoryOccupancySummaryDataContract>
    {
        private readonly IMedicalHistoryQueryService _medicalHistoryQueryService = medicalHistoryQueryService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetListMedicalHistoryOccupancySummaryDataContract> Handle(
            ConsumeContext<GetListMedicalHistoryOccupancySummaryByIdsEvent> context)
        {
            var medicalHistoryDTOs = await _medicalHistoryQueryService
                .GetAllOccupancySummariesByIdsAsync(context.Message.Ids);

            var medicalHistoryContracts = _mapper.Map<List<GetMedicalHistoryOccupancySummaryContract>>(medicalHistoryDTOs);

            return new() { Data = medicalHistoryContracts };
        }
    }
}
