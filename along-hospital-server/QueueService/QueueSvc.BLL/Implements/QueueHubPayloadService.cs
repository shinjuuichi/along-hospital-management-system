using AutoMapper;
using QueueSvc.BLL.DTOs.GetQueueDTOs;
using QueueSvc.BLL.DTOs.OtherDTOs;
using QueueSvc.BLL.DTOs.QueueHubDTOs;
using QueueSvc.BLL.Interfaces;
using QueueSvc.DAL.Enums;

namespace QueueSvc.BLL.Implements
{
    public class QueueHubPayloadService(
        IMapper mapper,
        IQueueQueryService queueQueryService,
        IQueueMessageBusService queueMessageBusService
        ) : IQueueHubPayloadService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IQueueQueryService _queueQueryService = queueQueryService;
        private readonly IQueueMessageBusService _queueMessageBusService = queueMessageBusService;

        public async Task<QueueHubSnapshotDTO> GetQueueSnapshotAsync(int? roomId)
        {
            var queueDTOs = await _queueQueryService.GetAllByTodayRoomIdAsync(roomId);
            return await this.MapQueueDTOsToSnapshotAsync(roomId, queueDTOs);
        }

        public async Task<QueueHubSnapshotDTO> BuildQueueCreatedPayloadAsync(int queueNumber, int? roomId)
        {
            var payload = await this.GetQueueSnapshotAsync(roomId);
            //var roomString = roomId == null ? "reception room" : $"room {payload.Room?.Code ?? roomId.ToString()}";
            //payload.Message = $"New queue number {queueNumber} has been created in {roomString}";
            return payload;
        }

        public async Task<QueueHubSnapshotDTO> BuildListQueueCreatedPayloadAsync(int numberOfCreatedQueue)
        {
            var payload = await this.GetQueueSnapshotAsync(roomId: null);
            //payload.Message = $"{numberOfCreatedQueue} new queues have been created";
            return payload;
        }

        public async Task<QueueHubSnapshotDTO> BuildQueueUpdatedPayloadAsync(int queueNumber, int? roomId)
        {
            var payload = await this.GetQueueSnapshotAsync(roomId);
            //payload.Message = $"Queue number {queueNumber} has been updated";
            return payload;
        }

        public async Task<QueueHubSnapshotDTO> BuildQueueStatusUpdatedPayloadAsync(int queueNumber, QueueStatusEnum status, int? roomId)
        {
            var payload = await this.GetQueueSnapshotAsync(roomId);
            //payload.Message = $"Queue number {queueNumber} has been updated to status {status}";
            return payload;
        }

        private async Task<QueueHubSnapshotDTO> MapQueueDTOsToSnapshotAsync(int? roomId, List<GetQueueDTO> queueDTOs)
        {
            GetRoomDTO? roomDTO = null;
            GetStaffDTO? staffDTO = null;
            if (roomId.HasValue)
            {
                (var roomContract, var doctorContract) = await _queueMessageBusService.GetRoomAndDoctorContractByRoomIdAsync(roomId.Value);
                roomDTO = _mapper.Map<GetRoomDTO>(roomContract);
                staffDTO = doctorContract != null ? _mapper.Map<GetStaffDTO>(doctorContract) : null;
            }

            return new QueueHubSnapshotDTO
            {
                RoomId = roomId,
                Room = roomDTO,
                DoctorId = staffDTO?.Id,
                Doctor = staffDTO,
                Queues = queueDTOs,
                Summary = new QueueHubSummaryDTO
                {
                    Total = queueDTOs.Count,
                    Waiting = this.CountByStatus(queueDTOs, nameof(QueueStatusEnum.Waiting)),
                    Called = this.CountByStatus(queueDTOs, nameof(QueueStatusEnum.Called)),
                    InProgress = this.CountByStatus(queueDTOs, nameof(QueueStatusEnum.InProgress)),
                    AwaitingResults = this.CountByStatus(queueDTOs, nameof(QueueStatusEnum.AwaitingResults)),
                    Completed = this.CountByStatus(queueDTOs, nameof(QueueStatusEnum.Completed)),
                    Cancelled = this.CountByStatus(queueDTOs, nameof(QueueStatusEnum.Cancelled)),
                }
            };
        }

        private int CountByStatus(List<GetQueueDTO> queues, string status)
        {
            return queues.Count(queue => string.Equals(queue.QueueStatus, status, StringComparison.OrdinalIgnoreCase));
        }
    }
}
