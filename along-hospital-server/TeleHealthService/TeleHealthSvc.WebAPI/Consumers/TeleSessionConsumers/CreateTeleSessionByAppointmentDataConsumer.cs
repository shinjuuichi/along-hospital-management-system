using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.TeleSessionContracts;
using MessageBroker.Events.TeleHealthEvents.TeleSessionEvents;
using SharedLibrary.Base.MessageBuses;
using TeleHealthSvc.BLL.DTOs.TeleSessionDTOs;
using TeleHealthSvc.BLL.Interfaces;

namespace TeleHealthSvc.WebAPI.Consumers.TeleSessionConsumers
{
    public class CreateTeleSessionByAppointmentDataConsumer(
        ITeleSessionService teleSessionService,
        IMapper mapper)
        : RequestConsumer<CreateTeleSessionByAppointmentDataEvent, CreateTeleSessionByAppointmentDataContract>
    {
        private readonly ITeleSessionService _teleSessionService = teleSessionService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<CreateTeleSessionByAppointmentDataContract> Handle(ConsumeContext<CreateTeleSessionByAppointmentDataEvent> context)
        {
            var createTeleSessionRequestDTO = _mapper.Map<CreateTeleSessionRequestDTO>(context.Message);
            await _teleSessionService.CreateTeleSessionByAppointmentDataAsync(createTeleSessionRequestDTO);

            return new CreateTeleSessionByAppointmentDataContract();
        }
    }
}
