using AutoMapper;
using FeedbackSvc.BLL.DTOs.FeedbackDTOs;
using FeedbackSvc.BLL.Interfaces.Management;
using FeedbackSvc.DAL.Models;
using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Contracts.MedicineContracts;
using MessageBroker.Events.AuthAccountEvents.GetUserDataEvents;
using MessageBroker.Events.MedicineEvents;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Base.Services;
using SharedLibrary.Commons.Filters;
using SharedLibrary.Commons.Results;

namespace FeedbackSvc.BLL.Implements.Management
{
    public class FeedbackManagementService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMessageBus messageBus)
        : BaseService<Feedback, CreateFeedbackDTO, UpdateFeedbackDTO, GetFeedbackAndMedicineDTO>(
            unitOfWork,
            mapper), IFeedbackManagementService
    {
        private readonly IMessageBus _messageBus = messageBus;

        public override async Task<PaginationResult<GetFeedbackAndMedicineDTO>> GetAllPaginatedAsync(FilterDTO filterDTO)
        {
            var (total, feedbacks) = await _repository.GetAllPaginatedAsync(
                filterDTO.Filter,
                filterDTO.Sort,
                filterDTO.Page,
                filterDTO.PageSize,
                [nameof(Feedback.FeedbackResponds)]
            );

            var feedbackDtos = _mapper.Map<List<GetFeedbackAndMedicineDTO>>(feedbacks);

            var patientIds = feedbackDtos.Select(x => x.PatientId).Distinct().ToList();
            var medicineIds = feedbackDtos.Select(x => x.GetMedicineDTO!.MedicineId).Distinct().ToList();
            var staffIds = feedbackDtos
                .Where(x => x.FeedbackResponds != null)
                .SelectMany(x => x.FeedbackResponds!)
                .Select(x => x.ResponderId)
                .Distinct()
                .ToList();

            var patientTask = _messageBus.RequestAsync<GetListUserDataByUserIdsEvent, GetListUserDataByUserIdsContract>(new()
            {
                UserIds = patientIds
            });

            var medicineTask = _messageBus.RequestAsync<GetListMedicineDataByIdsEvent, GetListMedicineDataByIdsContract>(new()
            {
                Ids = medicineIds
            });

            var staffTask = _messageBus.RequestAsync<GetListUserDataByUserIdsEvent, GetListUserDataByUserIdsContract>(new()
            {
                UserIds = staffIds
            });

            await Task.WhenAll(patientTask, medicineTask, staffTask);

            var patientDict = patientTask.Result.Data.ToDictionary(x => x.UserId);
            var medicineDict = medicineTask.Result.Data.ToDictionary(x => x.Id);
            var staffDict = staffTask.Result.Data.ToDictionary(x => x.UserId);

            foreach (var dto in feedbackDtos)
            {
                if (patientDict.TryGetValue(dto.PatientId, out var patient))
                {
                    _mapper.Map(patient, dto);
                }

                if (medicineDict.TryGetValue(dto.GetMedicineDTO!.MedicineId, out var medicine))
                {
                    _mapper.Map(medicine, dto);
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

            return new PaginationResult<GetFeedbackAndMedicineDTO>(total, filterDTO.PageSize, feedbackDtos);
        }
    }
}