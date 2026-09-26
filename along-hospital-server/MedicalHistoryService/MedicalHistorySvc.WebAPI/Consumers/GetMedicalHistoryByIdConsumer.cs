using AutoMapper;
using MassTransit;
using MedicalHistorySvc.BLL.Interfaces;
using MessageBroker.Contracts.MedicalHistoryContracts;
using MessageBroker.Events.MedicalHistoryEvents;
using SharedLibrary.Base.MessageBuses;

namespace MedicalHistorySvc.WebAPI.Consumers
{
    public class GetMedicalHistoryByIdConsumer(
        IMedicalHistoryQueryService medicalHistoryService,
        IMapper mapper)
        : RequestConsumer<GetMedicalHistoryByIdEvent, GetMedicalHistoryContract>
    {
        private readonly IMedicalHistoryQueryService _medicalHistoryService = medicalHistoryService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetMedicalHistoryContract> Handle(ConsumeContext<GetMedicalHistoryByIdEvent> context)
        {
            var medicalHistoryId = context.Message.Id;
            var notIncludeData = false;

            var medicalHistoryDTO = await _medicalHistoryService.GetByIdAsync(medicalHistoryId, notIncludeData);
            var medicalHistoryContract = _mapper.Map<GetMedicalHistoryContract>(medicalHistoryDTO);

            return medicalHistoryContract;
        }
    }
}