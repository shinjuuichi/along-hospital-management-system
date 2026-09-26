using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.TeleSessionContracts;
using MessageBroker.Events.TeleHealthEvents.TeleRoomEvents;
using SharedLibrary.Base.MessageBuses;
using TeleHealthSvc.BLL.Interfaces;

namespace TeleHealthSvc.WebAPI.Consumers.TeleRoomConsumers
{
    public class GetTeleRoomByIdConsumer(ITeleRoomService teleRoomService, IMapper mapper)
        : RequestConsumer<GetTeleRoomByIdEvent, GetTeleRoomContract>
    {
        private readonly ITeleRoomService _teleRoomService = teleRoomService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetTeleRoomContract> Handle(ConsumeContext<GetTeleRoomByIdEvent> context)
        {
            var teleRoomDTO = await _teleRoomService.GetByIdAsync(context.Message.Id);
            return _mapper.Map<GetTeleRoomContract>(teleRoomDTO);
        }
    }
}
