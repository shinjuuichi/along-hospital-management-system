using AutoMapper;
using InpatientResourceSvc.BLL.Interfaces;
using MassTransit;
using MessageBroker.Contracts.InPatientResourceContracts;
using MessageBroker.Events.InPatientResourceEvents;
using SharedLibrary.Base.MessageBuses;

namespace InpatientResourceSvc.WebAPI.Consumers
{
    public class GetBedOccupancyByMedicalHistoryIdConsumer(
        IBedOccupancyService bedOccupancyService,
        IMapper mapper)
        : RequestConsumer<GetBedOccupancyByMedicalHistoryIdEvent, GetBedOccupancyByMedicalHistoryIdContract>
    {
        private readonly IBedOccupancyService _bedOccupancyService = bedOccupancyService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetBedOccupancyByMedicalHistoryIdContract> Handle(
            ConsumeContext<GetBedOccupancyByMedicalHistoryIdEvent> context)
        {
            var medicalHistoryId = context.Message.MedicalHistoryId;
            var bedOccupancyDto = await _bedOccupancyService.GetByMedicalHistoryIdAsync(medicalHistoryId);

            return bedOccupancyDto != null
                ? _mapper.Map<GetBedOccupancyByMedicalHistoryIdContract>(bedOccupancyDto)
                : new GetBedOccupancyByMedicalHistoryIdContract();
        }
    }
}
