using AutoMapper;
using InpatientResourceSvc.BLL.Interfaces;
using MassTransit;
using MessageBroker.Contracts.InPatientResourceContracts;
using MessageBroker.Events.InPatientResourceEvents;
using SharedLibrary.Base.MessageBuses;

namespace InpatientResourceSvc.WebAPI.Consumers
{
    public class GetListBedOccupancyDataByMedicalHistoryIdConsumer(
        IBedOccupancyService bedOccupancyService,
        IMapper mapper)
        : RequestConsumer<GetListBedOccupancyDataByMedicalHistoryIdEvent, GetListBedOccupancyDataContract>
    {
        private readonly IBedOccupancyService _bedOccupancyService = bedOccupancyService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetListBedOccupancyDataContract> Handle(
            ConsumeContext<GetListBedOccupancyDataByMedicalHistoryIdEvent> context)
        {
            var medicalHistoryId = context.Message.MedicalHistoryId;
            var bedOccupancyDtos = await _bedOccupancyService.GetAllByMedicalHistoryIdAsync(medicalHistoryId);

            return new GetListBedOccupancyDataContract
            {
                Data = _mapper.Map<List<GetBedOccupancyByMedicalHistoryIdContract>>(bedOccupancyDtos)
            };
        }
    }
}
