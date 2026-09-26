using AutoMapper;
using QueueSvc.BLL.DTOs;
using QueueSvc.BLL.DTOs.CreateQueueDTOs;
using QueueSvc.BLL.DTOs.GetQueueDTOs;
using QueueSvc.BLL.Interfaces;
using QueueSvc.BLL.StateMachines;
using QueueSvc.BLL.Utils;
using QueueSvc.DAL.Enums;
using QueueSvc.DAL.Models;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Utils;

namespace QueueSvc.BLL.Implements
{
    public class QueueCommandService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IQueueQueryService queueQueryService,
        IQueueMessageBusService queueMessageBusService,
        IQueueCacheService queueCacheService,
        IQueueHubService queueHubService)
            : IQueueCommandService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        private readonly IQueueQueryService _queueQueryService = queueQueryService;
        private readonly IQueueMessageBusService _queueMessageBusService = queueMessageBusService;
        private readonly IQueueCacheService _queueCacheService = queueCacheService;
        private readonly IQueueHubService _queueHubService = queueHubService;

        private readonly IGenericRepository<Queue> _queueRepository = unitOfWork.Repository<Queue>();
        private readonly string[] _includes = [nameof(Queue.QueueEvents)];

        public async Task<GetQueueDTO> CreateAsync()
        {
            var queue = await this.InternalCreateNewQueueAsync(roomId: null);

            var createdQueue = await _queueRepository.AddAsync(queue);
            await _unitOfWork.SaveChangeAsync();

            await _queueHubService.PublishQueueCreatedAsync(createdQueue.QueueNumber, createdQueue.RoomId);

            return _mapper.Map<GetQueueDTO>(createdQueue);
        }

        public async Task<GetQueueDTO> CreateFromQRDataAsync(CreateQueueFromQRDataDTO createDTO)
        {
            var medicalHistoryId = createDTO.MedicalHistoryId;
            var roomId = createDTO.RoomId;

            // Validate medical history status
            var medicalHistoryContract = await _queueMessageBusService.GetMedicalHistoryAndValidateStatusAsync(medicalHistoryId, ModelConstants.MEDICAL_HISTORY_STATUS_DRAFT);

            // Check if patient already has an active queue in any room
            var hasActiveQueue = await _queueRepository.AnyAsync(q =>
                q.MedicalHistoryId == medicalHistoryId
                && (q.QueueStatus == QueueStatusEnum.Waiting
                    || q.QueueStatus == QueueStatusEnum.Called
                    || q.QueueStatus == QueueStatusEnum.InProgress));
            if (hasActiveQueue)
            {
                throw new InvalidDataException("Patient already has an active queue.");
            }

            // Check if patient has a queue in the same room today
            // If the queue is in status Completed or Cancelled, allow patient to create a new queue.
            // If the queue is in status AwaitingResults, update it to Waiting to allow patient re-enter the queue without creating a new one.
            // Other status => throw exception to prevent patient from creating multiple queues in the same day.
            var todayDate = DateTime.UtcNow.Date;
            var tomorrowDate = todayDate.AddDays(1);

            var existedQueueInTodayRoom = (await _queueRepository.GetAllAsync(q =>
                q.MedicalHistoryId == medicalHistoryId
                && q.RoomId == roomId
                && q.QueueDate >= todayDate
                && q.QueueDate < tomorrowDate, _includes))
                .OrderByDescending(q => q.QueueDate)
                .ThenByDescending(q => q.QueueOrder)
                .FirstOrDefault();

            if (existedQueueInTodayRoom != null)
            {
                if (existedQueueInTodayRoom.QueueStatus == QueueStatusEnum.AwaitingResults)
                {
                    await this.InternalUpdateStatusAsync(existedQueueInTodayRoom, QueueStatusEnum.Waiting);
                    _queueRepository.Update(existedQueueInTodayRoom);
                    await _unitOfWork.SaveChangeAsync();

                    await _queueHubService.PublishQueueStatusUpdatedAsync(
                        existedQueueInTodayRoom.QueueNumber,
                        existedQueueInTodayRoom.QueueStatus,
                        existedQueueInTodayRoom.RoomId);

                    return _mapper.Map<GetQueueDTO>(existedQueueInTodayRoom);
                }

                if (existedQueueInTodayRoom.QueueStatus != QueueStatusEnum.Completed
                    && existedQueueInTodayRoom.QueueStatus != QueueStatusEnum.Cancelled)
                {
                    throw new InvalidDataException("Patient cannot re-enter queue in current status.");
                }
            }

            // Get specialty contract for snapshot data
            var specialtyContract = await _queueMessageBusService.GetSpecialtyContractAsync(medicalHistoryContract.SpecialtyId);

            createDTO.SpecialtyId = medicalHistoryContract.SpecialtyId;
            createDTO.MedicalHistorySnapshot = _mapper.Map<CreateMedicalHistorySnapshotDTO>(medicalHistoryContract);
            createDTO.SpecialtySnapshot = _mapper.Map<CreateSpecialtySnapshotDTO>(specialtyContract);

            // Create new queue
            var queue = await this.InternalCreateNewQueueAsync(roomId);
            _mapper.Map(createDTO, queue);

            var createdQueue = await _queueRepository.AddAsync(queue);
            await _unitOfWork.SaveChangeAsync();

            await _queueHubService.PublishQueueCreatedAsync(createdQueue.QueueNumber, createdQueue.RoomId);

            return _mapper.Map<GetQueueDTO>(createdQueue);
        }

        public async Task AssignMedicalHistoryAsync(int queueId, AssignMedicalHistoryToQueueDTO assignDTO)
        {
            var medicalHistoryContract = await _queueMessageBusService.GetMedicalHistoryAndValidateStatusAsync(assignDTO.MedicalHistoryId, ModelConstants.MEDICAL_HISTORY_STATUS_PENDING_PAYMENT);
            var specialtyContract = await _queueMessageBusService.GetSpecialtyContractAsync(medicalHistoryContract.SpecialtyId);

            assignDTO.SpecialtyId = medicalHistoryContract.SpecialtyId;
            assignDTO.MedicalHistorySnapshot = _mapper.Map<CreateMedicalHistorySnapshotDTO>(medicalHistoryContract);
            assignDTO.SpecialtySnapshot = _mapper.Map<CreateSpecialtySnapshotDTO>(specialtyContract);

            var queue = await _queueRepository.GetByIdAsync(queueId)
                ?? throw new DataNotFoundException(typeof(Queue), queueId);

            if (queue.QueueStatus != QueueStatusEnum.InProgress)
            {
                throw new InvalidDataException("Can only assign medical history to queue in InProgress status.");
            }

            _mapper.Map(assignDTO, queue);

            _queueRepository.Update(queue);
            await _unitOfWork.SaveChangeAsync();
            await _queueHubService.PublishQueueUpdatedAsync(queue.QueueNumber, queue.RoomId);
        }

        public async Task UpdateStatusAsync(int id, QueueStatusEnum status)
        {
            var queue = await _queueRepository.GetByIdAsync(id, _includes)
                ?? throw new DataNotFoundException(typeof(Queue), id);

            await _queueMessageBusService.ValidateStaffActionAsync(queue.RoomId);

            switch (status)
            {
                case QueueStatusEnum.InProgress:
                    var isOnlineQueue = queue.RoomId == null && queue.AppointmentId.HasValue;
                    var hasInProgressQueue = await _queueQueryService.HasAnyInProgressQueueTodayAsync(queue.RoomId, isOnlineQueue);
                    if (hasInProgressQueue)
                    {
                        throw new DataConflictException("There is already a queue in progress. Please finish it before starting another one.");
                    }
                    break;
                case QueueStatusEnum.AwaitingResults:
                    if (!queue.MedicalHistoryId.HasValue || queue.MedicalHistoryId.Value <= 0)
                    {
                        throw new InvalidDataException("Cannot set queue to awaiting results without medical history.");
                    }
                    break;
                case QueueStatusEnum.Completed:
                    if (!queue.MedicalHistoryId.HasValue || queue.MedicalHistoryId.Value <= 0)
                    {
                        throw new InvalidDataException("Cannot complete queue without medical history.");
                    }
                    break;
            }

            await this.InternalUpdateStatusAsync(queue, status);
            _queueRepository.Update(queue);
            await _unitOfWork.SaveChangeAsync();

            await _queueHubService.PublishQueueStatusUpdatedAsync(queue.QueueNumber, status, queue.RoomId);
        }

        public async Task RunAppointmentQueueingBackgroundAsync()
        {
            // Get all appointments today to check if there is any appointment that needs to create queue
            var now = DateTime.UtcNow.ConvertTimeToTimeZone();
            var nowDate = DateOnly.FromDateTime(now);
            var nowTime = TimeOnly.FromDateTime(now);
            var appointments = await _queueMessageBusService.GetAllAppointmentContractsByDateAsync(nowDate);

            var appointmentsToCreateQueue = appointments
                .Where(a =>
                   a.Date == nowDate
                && a.Time <= nowTime
                && a.MedicalHistoryId.HasValue
                && a.AppointmentStatus == ModelConstants.APPOINTMENT_STATUS_SCHEDULED
                && a.AppointmentMeetingType == ModelConstants.APPOINTMENT_MEETING_TYPE_IN_PERSON
                && a.AppointmentPaymentStatus == ModelConstants.APPOINTMENT_PAYMENT_STATUS_COMPLETED).ToList();

            if (appointmentsToCreateQueue.Count == 0)
            {
                return;
            }

            // Create queues from appointment data
            var appointmentDataToCreate = _mapper.Map<List<CreateQueueFromAppointmentDataDTO>>(appointmentsToCreateQueue);
            var createdQueuesData = await this.CreateFromAppointmentsDataAsync(appointmentDataToCreate);

            // Update appointment status to Completed after creating queue successfully
            var createdAppointmentIds = createdQueuesData.Select(c => c.AppointmentId).ToList();
            var appointmentsToUpdate = appointmentsToCreateQueue.Where(a => createdAppointmentIds.Contains(a.Id)).ToList();

            foreach (var appointment in appointmentsToUpdate)
            {
                var updatedAppointment = appointment with
                {
                    AppointmentStatus = ModelConstants.APPOINTMENT_STATUS_COMPLETED
                };

                await _queueCacheService.UpdateAppointmentInCacheAsync(nowDate, appointment.Id, updatedAppointment);
            }

            await _queueMessageBusService.UpdateListAppointmentToCompletedAsync(createdAppointmentIds);
        }

        private async Task<List<CreateQueueFromAppointmentDataDTO>> CreateFromAppointmentsDataAsync(List<CreateQueueFromAppointmentDataDTO> createDTO)
        {
            if (createDTO.Count == 0)
            {
                return [];
            }

            // Get medical history and specialty contracts for snapshot data
            var medicalHistoryIds = createDTO
                .Where(q => q.MedicalHistoryId.HasValue)
                .Select(q => q.MedicalHistoryId!.Value)
                .Distinct()
                .ToList();
            var specialtyIds = createDTO
                .Select(q => q.SpecialtyId)
                .Distinct()
                .ToList();

            var medicalHistoryContracts = await _queueMessageBusService.GetAllMedicalHistoryContractsByIdsAsync(medicalHistoryIds);
            var specialtyContracts = await _queueMessageBusService.GetAllSpecialtyContractsByIdsAsync(specialtyIds);

            var medicalHistoryDict = medicalHistoryContracts.ToDictionary(mh => mh.Id, mh => mh);
            var specialtyDict = specialtyContracts.ToDictionary(s => s.Id, s => s);

            // Create queue list to add range to database
            var createdQueueFromAppointments = new List<CreateQueueFromAppointmentDataDTO>();
            var createdQueues = new List<Queue>(createDTO.Count);

            (var lastMaxNumber, var lastMaxOrder) = await _queueQueryService.GetQueueAggregatesTodayAsync(null);

            foreach (var item in createDTO)
            {
                if (!item.MedicalHistoryId.HasValue || !medicalHistoryDict.TryGetValue(item.MedicalHistoryId.Value, out var medicalHistoryContract))
                {
                    continue;
                }

                if (!specialtyDict.TryGetValue(item.SpecialtyId, out var specialtyContract))
                {
                    continue;
                }

                item.MedicalHistorySnapshot = _mapper.Map<CreateMedicalHistorySnapshotDTO>(medicalHistoryContract);
                item.SpecialtySnapshot = _mapper.Map<CreateSpecialtySnapshotDTO>(specialtyContract);
                var queue = new Queue
                {
                    QueueNumber = ++lastMaxNumber,
                    QueueOrder = ++lastMaxOrder
                };

                _mapper.Map(item, queue);
                createdQueueFromAppointments.Add(item);
                createdQueues.Add(queue);
            }

            await _queueRepository.AddRangeAsync(createdQueues);
            await _unitOfWork.SaveChangeAsync();

            var numberOfCreatedQueues = createdQueues.Count;
            await _queueHubService.PublishListQueueCreatedAsync(numberOfCreatedQueues);

            return createdQueueFromAppointments;
        }

        private async Task<Queue> InternalCreateNewQueueAsync(int? roomId = null)
        {
            (int maxQueueNumber, int maxQueueOrder) = await _queueQueryService.GetQueueAggregatesTodayAsync(roomId);

            return new Queue
            {
                QueueNumber = maxQueueNumber + 1,
                QueueOrder = maxQueueOrder + 1,
                RoomId = roomId,
            };
        }

        private async Task InternalUpdateStatusAsync(Queue queue, QueueStatusEnum status)
        {
            var queueStatusStateMachine = new QueueStatusStateMachine(queue, _queueQueryService, _queueMessageBusService);
            if (!queueStatusStateMachine.CanFire(status))
            {
                throw new InvalidDataException($"Cannot change queue status from {queue.QueueStatus} to {status}");
            }

            try
            {
                queue.QueueEvents.Add(new QueueEvent
                {
                    FromStatus = queue.QueueStatus,
                    ToStatus = status,
                });
                await queueStatusStateMachine.FireAsync(status);
            }
            catch (Exception ex)
            {
                throw new InvalidDataException($"Failed to change queue status: {ex.Message}");
            }
        }
    }
}
