using AutoMapper;
using InpatientResourceSvc.BLL.Interfaces;
using MassTransit;
using MessageBroker.Contracts.InPatientResourceContracts;
using MessageBroker.Events.InPatientResourceEvents;
using SharedLibrary.Base.MessageBuses;

namespace InpatientResourceSvc.WebAPI.Consumers
{
    public class GetRoomByIdConsumer(IRoomService roomService, IMapper mapper)
        : RequestConsumer<GetRoomByIdEvent, GetRoomContract>
    {
        private readonly IRoomService _roomService = roomService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetRoomContract> Handle(ConsumeContext<GetRoomByIdEvent> context)
        {
            var roomDTO = await _roomService.GetByIdAsync(context.Message.Id);
            return _mapper.Map<GetRoomContract>(roomDTO);
        }
    }
}