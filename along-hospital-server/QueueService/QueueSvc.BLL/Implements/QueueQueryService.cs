using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QueueSvc.BLL.DTOs.GetQueueDTOs;
using QueueSvc.BLL.DTOs.OtherDTOs;
using QueueSvc.BLL.DTOs.QueueHubDTOs;
using QueueSvc.BLL.Interfaces;
using QueueSvc.DAL.Enums;
using QueueSvc.DAL.Models;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;

namespace QueueSvc.BLL.Implements
{
    public class QueueQueryService(
        IUnitOfWork unitOfWork,
        IQueueMessageBusService queueMessageBusService,
        IMapper mapper) : IQueueQueryService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IQueueMessageBusService _queueMessageBusService = queueMessageBusService;
        private readonly IGenericRepository<Queue> _queueRepository = unitOfWork.Repository<Queue>();

        private readonly string[] _includes = [nameof(Queue.QueueEvents)];

        public async Task<List<QueueHubSnapshotDTO>> GetAllTodayWorkingQueueAsync()
        {
            var roomData = await _queueMessageBusService.GetListTodayWorkingMedicalRoomAsync();

            var roomDict = roomData
                .GroupBy(x => x.RoomId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.DoctorId).FirstOrDefault());
            var roomIds = roomDict.Keys.ToList();
            var doctorIds = roomDict.Values
                .Where(id => id.HasValue)
                .Select(id => id!.Value)
                .Distinct()
                .ToList();

            var todayDate = DateTime.UtcNow.Date;
            var tomorrowDate = todayDate.AddDays(1);

            var queues = await _queueRepository.GetAllAsync(
                q => (q.RoomId == null || roomIds.Contains(q.RoomId.Value))
                     && q.QueueDate >= todayDate
                     && q.QueueDate < tomorrowDate,
                _includes);

            var roomContracts = await _queueMessageBusService.GetAllRoomContractsByIdsAsync(roomIds);
            var doctorContracts = await _queueMessageBusService.GetAllDoctorContractsByIdsAsync(doctorIds);

            var normalQueues = queues.Where(q => q.RoomId.HasValue).ToList();
            var receptionQueues = queues.Where(q => q.RoomId == null).ToList();

            var queueLookup = normalQueues.GroupBy(q => q.RoomId!.Value).ToDictionary(g => g.Key, g => g.ToList());

            var result = roomIds
                .Select(roomId =>
                {
                    roomDict.TryGetValue(roomId, out var doctorId);

                    var queueList = queueLookup.TryGetValue(roomId, out var list) ? list : [];

                    var queueDTOs = _mapper.Map<List<GetQueueDTO>>(queueList);
                    queueDTOs = this.SortAndReturnDTOs(queueDTOs);

                    GetRoomDTO? roomDTO = null;
                    GetStaffDTO? doctorDTO = null;

                    var roomContract = roomContracts.FirstOrDefault(rc => rc.Id == roomId);
                    roomDTO = roomContract != null ? _mapper.Map<GetRoomDTO>(roomContract) : null;

                    var doctorContract = doctorId.HasValue
                        ? doctorContracts.FirstOrDefault(dc => dc.UserId == doctorId.Value)
                        : null;

                    doctorDTO = doctorContract != null
                        ? _mapper.Map<GetStaffDTO>(doctorContract)
                        : null;

                    return new QueueHubSnapshotDTO
                    {
                        RoomId = roomId,
                        Room = roomDTO,
                        DoctorId = doctorId,
                        Doctor = doctorDTO,
                        Queues = queueDTOs
                    };
                })
                .OrderBy(x => x.RoomId)
                .ToList();

            var receptionDTOs = _mapper.Map<List<GetQueueDTO>>(receptionQueues);
            receptionDTOs = this.SortAndReturnDTOs(receptionDTOs);

            result.Add(new QueueHubSnapshotDTO
            {
                RoomId = null,
                Room = null,
                DoctorId = null,
                Doctor = null,
                Queues = receptionDTOs
            });

            result = result.OrderBy(x => x.RoomId ?? -1).ToList();

            return result;
        }

        public async Task<List<GetQueueDTO>> GetAllByTodayRoomIdAsync(int? roomId)
        {
            var todayDate = DateTime.UtcNow.Date;
            var tomorrowDate = todayDate.AddDays(1);

            var queues = await _queueRepository.GetAllAsync(
                q => q.RoomId == roomId
                && q.QueueDate >= todayDate
                && q.QueueDate < tomorrowDate, _includes);

            var queueDTOs = _mapper.Map<List<GetQueueDTO>>(queues);
            queueDTOs = this.SortAndReturnDTOs(queueDTOs);

            await this.RequestValueForQueueDTOsAsync(queueDTOs);
            return queueDTOs;
        }

        public async Task<bool> HasAnyInProgressQueueTodayAsync(int? roomId, bool isOnlineQueue)
        {
            var todayDate = DateTime.UtcNow.Date;
            var tomorrowDate = todayDate.AddDays(1);

            if (roomId.HasValue)
            {
                return await _queueRepository.AnyAsync(q => q.RoomId == roomId
                    && q.QueueStatus == QueueStatusEnum.InProgress
                    && q.QueueDate >= todayDate
                    && q.QueueDate < tomorrowDate);
            }

            return await _queueRepository.AnyAsync(q => q.RoomId == null
                && (isOnlineQueue ? q.AppointmentId.HasValue : !q.AppointmentId.HasValue)
                && q.QueueStatus == QueueStatusEnum.InProgress
                && q.QueueDate >= todayDate
                && q.QueueDate < tomorrowDate);
        }

        #region Get Metadata
        public async Task<(int maxNumber, int maxOrder)> GetQueueAggregatesTodayAsync(int? roomId)
        {
            var todayDate = DateTime.UtcNow.Date;
            var tomorrowDate = todayDate.AddDays(1);

            var result = await _queueRepository.GetAllQueryable()
                .Where(q => q.RoomId == roomId
                    && q.QueueDate >= todayDate
                    && q.QueueDate < tomorrowDate)
                .GroupBy(_ => true)
                .Select(g => new
                {
                    MaxNumber = g.Max(x => x.QueueNumber),
                    MaxOrder = g.Max(x => x.QueueOrder)
                })
                .FirstOrDefaultAsync();

            if (result == null)
            {
                return (0, 0);
            }

            return (result.MaxNumber, result.MaxOrder);
        }

        public async Task<int> GetLastQueueOrderByPriorityAsync(int? roomId, int priority)
        {
            var todayDate = DateTime.UtcNow.Date;
            var tomorrowDate = todayDate.AddDays(1);

            var maxQueueOrder = await _queueRepository.GetAllQueryable()
                .Where(q => q.RoomId == roomId
                         && q.Priority == priority
                         && q.QueueStatus == QueueStatusEnum.Waiting
                         && q.QueueDate >= todayDate
                         && q.QueueDate < tomorrowDate)
                .Select(q => (int?)q.QueueOrder)
                .MaxAsync();

            return maxQueueOrder ?? 0;
        }
        #endregion

        private async Task RequestValueForQueueDTOsAsync(List<GetQueueDTO> getQueueDTOs)
        {
            var distinctPatientIds = getQueueDTOs
               .Where(q => q.MedicalHistorySnapshot != null)
               .Select(q => q.MedicalHistorySnapshot!.PatientId)
               .Distinct()
               .ToList();

            var patientContracts = await _queueMessageBusService.GetAllPatientContractsByIdsAsync(distinctPatientIds);
            var patientDict = patientContracts.ToDictionary(pc => pc.UserId);

            foreach (var queueDTO in getQueueDTOs)
            {
                if (queueDTO.MedicalHistorySnapshot != null)
                {
                    var patientId = queueDTO.MedicalHistorySnapshot.PatientId;
                    if (patientDict.TryGetValue(patientId, out var patientContract))
                    {
                        queueDTO.MedicalHistorySnapshot.Patient = _mapper.Map<GetPatientDTO>(patientContract);
                    }
                }
            }
        }

        private List<GetQueueDTO> SortAndReturnDTOs(List<GetQueueDTO> queueDTOs)
        {
            return queueDTOs
                .OrderBy(queue => this.GetQueueStatusSortRank(queue.QueueStatus))
                .ThenByDescending(queue => queue.Priority)
                .ThenBy(queue => queue.QueueOrder)
                .ToList();
        }

        private int GetQueueStatusSortRank(string? queueStatus)
        {
            if (!Enum.TryParse<QueueStatusEnum>(queueStatus, true, out var parsedStatus))
            {
                return 3;
            }

            return parsedStatus switch
            {
                QueueStatusEnum.InProgress => 0,
                QueueStatusEnum.Called => 1,
                QueueStatusEnum.Waiting => 2,
                _ => 3
            };
        }
    }
}
