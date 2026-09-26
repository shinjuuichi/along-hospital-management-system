using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.TeleSessionContracts;
using MessageBroker.Events.TeleHealthEvents.TeleSessionEvents;
using SharedLibrary.Base.MessageBuses;
using TeleHealthSvc.BLL.Interfaces;

namespace TeleHealthSvc.WebAPI.Consumers.TeleSessionConsumers
{
    public class GetTeleSessionByListAppointmentIdConsumer(
        ITeleSessionService teleSessionService,
        IMapper mapper) : RequestConsumer<GetTeleSessionByAppointmentIdsEvent, GetListTeleSessionByAppointmentIdsContract>
    {
        private readonly ITeleSessionService _teleSessionService = teleSessionService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetListTeleSessionByAppointmentIdsContract> Handle(ConsumeContext<GetTeleSessionByAppointmentIdsEvent> context)
        {
            var teleSessions = await _teleSessionService.GetTeleSessionByListAppointmentIdAsync(context.Message.Appointments);

            return new GetListTeleSessionByAppointmentIdsContract
            {
                TeleSessions = _mapper.Map<List<GetTeleSessionByAppointmentIdContract>>(teleSessions)
            };
        }
    }
}