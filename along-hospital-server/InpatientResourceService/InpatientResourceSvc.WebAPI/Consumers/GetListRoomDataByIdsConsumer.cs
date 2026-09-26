using AutoMapper;
using InpatientResourceSvc.BLL.Interfaces;
using MassTransit;
using MessageBroker.Contracts.InPatientResourceContracts;
using MessageBroker.Events.InPatientResourceEvents;
using SharedLibrary.Base.MessageBuses;

namespace InpatientResourceSvc.WebAPI.Consumers
{
    public class GetListRoomDataByIdsConsumer(IRoomService roomService, IMapper mapper)
        : RequestConsumer<GetListRoomDataByIdsEvent, GetListRoomDataContract>
    {
        private readonly IRoomService _roomService = roomService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetListRoomDataContract> Handle(ConsumeContext<GetListRoomDataByIdsEvent> context)
        {
            var roomIds = context.Message.Ids;

            var roomDTOs = await _roomService.GetAllByIdsAsync(roomIds);
            var roomContracts = _mapper.Map<List<GetRoomContract>>(roomDTOs);

            return new() { Data = roomContracts };
        }
    }
}