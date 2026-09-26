using AppointmentSvc.BLL.Interfaces;
using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.AppointmentContracts;
using MessageBroker.Events.AppointmentEvents;
using SharedLibrary.Base.MessageBuses;

namespace AppointmentSvc.WebAPI.Consumers
{
    public class GetAllTimeSlotsConsumer(
        ITimeSlotService timeSlotService,
        IMapper mapper)
        : RequestConsumer<GetAllTimeSlotsEvent, GetAllTimeSlotsContract>
    {
        private readonly ITimeSlotService _timeSlotService = timeSlotService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetAllTimeSlotsContract> Handle(ConsumeContext<GetAllTimeSlotsEvent> context)
        {
            var timeSlotDTOs = await _timeSlotService.GetAllAsync();
            var orderedTimeSlotDTOs = timeSlotDTOs
                .OrderBy(timeSlot => timeSlot.Time)
                .ToList();

            var contracts = _mapper.Map<List<GetTimeSlotContract>>(orderedTimeSlotDTOs);

            return new GetAllTimeSlotsContract
            {
                Data = contracts
            };
        }
    }
}