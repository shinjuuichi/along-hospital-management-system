using AutoMapper;
using FeedbackSvc.BLL.DTOs.FeedbackDTOs;
using FeedbackSvc.BLL.Interfaces;
using FeedbackSvc.DAL.Enums;
using FeedbackSvc.DAL.Models;
using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Contracts.PatientContracts;
using MessageBroker.Events.AuthAccountEvents.GetUserDataEvents;
using MessageBroker.Events.FeedbackEvents;
using MessageBroker.Events.MedicineEvents;
using MessageBroker.Events.PatientEvents;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Base.Services;
using SharedLibrary.Services.Interfaces;

namespace FeedbackSvc.BLL.Implements
{
    public class FeedbackService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMessageBus messageBus,
        ICurrentUserService currentUserService)
        : BaseService<Feedback, CreateFeedbackDTO, UpdateFeedbackDTO, GetFeedbackDTO>(
            unitOfWork,
            mapper), IFeedbackService
    {
        private readonly IMessageBus _messageBus = messageBus;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        #region Override Methods
        public override async Task<GetFeedbackDTO> CreateAsync(CreateFeedbackDTO createFeedbackDTO)
        {
            var patientId = _currentUserService.UserId;

            var medicineTask = _messageBus.RequestAsync<CheckMedicineExistByIdEvent, CheckMedicineExistByIdContract>(new() { MedicineId = createFeedbackDTO.MedicineId });
            var patientTask = _messageBus.RequestAsync<CheckPatientExistByIdEvent, CheckPatientExistByIdContract>(new() { PatientId = patientId });

            await Task.WhenAll(medicineTask, patientTask);

            var getFeedbackDTO = await base.CreateAsync(createFeedbackDTO);

            var predictFeedbackTypeEvent = _mapper.Map<PredictFeedbackTypeEvent>(getFeedbackDTO);
            await _messageBus.PublishAsync<PredictFeedbackTypeEvent>(predictFeedbackTypeEvent);

            var predictFeedbackToxicEvent = _mapper.Map<PredictFeedbackToxicEvent>(getFeedbackDTO);
            await _messageBus.PublishAsync<PredictFeedbackToxicEvent>(predictFeedbackToxicEvent);

            return getFeedbackDTO;
        }

        public override async Task<GetFeedbackDTO> UpdateAsync(int id, UpdateFeedbackDTO updateFeedbackDTO)
        {
            var patientId = _currentUserService.UserId;

            var medicineTask = _messageBus.RequestAsync<CheckMedicineExistByIdEvent, CheckMedicineExistByIdContract>(new() { MedicineId = updateFeedbackDTO.MedicineId });
            var patientTask = _messageBus.RequestAsync<CheckPatientExistByIdEvent, CheckPatientExistByIdContract>(new() { PatientId = patientId });

            await Task.WhenAll(medicineTask, patientTask);

            var getFeedbackDTO = await base.UpdateAsync(id, updateFeedbackDTO);

            var predictFeedbackTypeEvent = _mapper.Map<PredictFeedbackTypeEvent>(getFeedbackDTO);
            await _messageBus.PublishAsync<PredictFeedbackTypeEvent>(predictFeedbackTypeEvent);

            var predictFeedbackToxicEvent = _mapper.Map<PredictFeedbackToxicEvent>(getFeedbackDTO);
            await _messageBus.PublishAsync<PredictFeedbackToxicEvent>(predictFeedbackToxicEvent);

            return getFeedbackDTO;
        }
        #endregion

        #region Primary Methods
        public async Task<List<GetFeedbackDTO>> GetFeedbackByMedicineIdAsync(int medicineId)
        {
            var feedbacks = await _repository.GetAllAsync(f => f.MedicineId == medicineId, includes: [nameof(Feedback.FeedbackResponds)]);
            var feedbackDTOs = _mapper.Map<List<GetFeedbackDTO>>(feedbacks);

            var patientIds = feedbackDTOs.Select(x => x.PatientId).Distinct().ToList();
            var staffIds = feedbackDTOs
                .Where(x => x.FeedbackResponds != null)
                .SelectMany(x => x.FeedbackResponds!)
                .Select(x => x.ResponderId)
                .Distinct()
                .ToList();

            var patientTask = _messageBus.RequestAsync<GetListUserDataByUserIdsEvent, GetListUserDataByUserIdsContract>(new()
            {
                UserIds = patientIds
            });

            var staffTask = _messageBus.RequestAsync<GetListUserDataByUserIdsEvent, GetListUserDataByUserIdsContract>(new()
            {
                UserIds = staffIds
            });

            await Task.WhenAll(patientTask, staffTask);

            var patientDict = patientTask.Result.Data.ToDictionary(x => x.UserId);
            var staffDict = staffTask.Result.Data.ToDictionary(x => x.UserId);

            foreach (var dto in feedbackDTOs)
            {
                if (patientDict.TryGetValue(dto.PatientId, out var patient))
                {
                    _mapper.Map(patient, dto);
                }

                if (dto.FeedbackResponds != null)
                {
                    foreach (var respondDto in dto.FeedbackResponds)
                    {
                        if (staffDict.TryGetValue(respondDto.ResponderId, out var staff))
                        {
                            _mapper.Map(staff, respondDto);
                        }
                    }
                }
            }

            return feedbackDTOs;
        }

        public async Task BanFeedbackByUserIdAsync(int userId)
        {
            var feedbacks = await _repository.GetAllAsync(f => f.CreatedBy == userId);

            foreach (var feedback in feedbacks)
            {
                feedback.FeedbackStatus = FeedbackStatusEnum.Hidden;
                _repository.Update(feedback);
            }

            await _unitOfWork.SaveChangeAsync();
        }

        public async Task BanFeedbackAsync(int id)
        {
            var feedback = await _repository.GetByIdAsync(id);
            if (feedback != null)
            {
                feedback.FeedbackStatus = FeedbackStatusEnum.Hidden;

                _repository.Update(feedback);
                await _unitOfWork.SaveChangeAsync();
            }
        }

        public async Task UpdateFeedbackTypeAsync(int id, string feedbackType)
        {
            if (!Enum.TryParse<FeedbackTypeEnum>(feedbackType, ignoreCase: true, out var parsedFeedbackType))
            {
                throw new InvalidDataException("Invalid feedback type");
            }

            var feedback = await _repository.GetByIdAsync(id);
            if (feedback != null)
            {
                feedback.FeedbackTypeEnum = parsedFeedbackType;
                _repository.Update(feedback);
                await _unitOfWork.SaveChangeAsync();
            }
        }

        public async Task UpdateFeedbackReplyStatusAsync(int id, string feedbackReplyStatus)
        {
            if (!Enum.TryParse<FeedbackReplyStatusEnum>(feedbackReplyStatus, ignoreCase: true, out var parsedFeedbackReplyStatus))
            {
                throw new InvalidDataException("Invalid feedback reply status");
            }

            var feedback = await _repository.GetByIdAsync(id);
            if (feedback != null)
            {
                feedback.FeedbackReplyStatus = parsedFeedbackReplyStatus;
                _repository.Update(feedback);
                await _unitOfWork.SaveChangeAsync();
            }
        }
        #endregion
    }
}