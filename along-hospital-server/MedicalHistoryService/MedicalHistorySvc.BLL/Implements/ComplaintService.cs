using AutoMapper;
using MedicalHistorySvc.BLL.DTOs.ComplaintDTOs;
using MedicalHistorySvc.BLL.FilterDTOs;
using MedicalHistorySvc.BLL.Interfaces;
using MedicalHistorySvc.BLL.StateMachines;
using MedicalHistorySvc.DAL.Enums;
using MedicalHistorySvc.DAL.Models;
using MessageBroker.Contracts.LLMContracts;
using MessageBroker.Events.LLMEvents;
using MessageBroker.Events.MedicalHistoryEvents;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Commons.Results;
using SharedLibrary.Services.Interfaces;
using SharedLibrary.Utils;

namespace MedicalHistorySvc.BLL.Implements
{
    public class ComplaintService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ICurrentUserService currentUserService,
        IComplaintPredictionApiService complaintPredictionApiService,
        IMessageBus messageBus)
            : IComplaintService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IComplaintPredictionApiService _complaintPredictionApiService = complaintPredictionApiService;
        private readonly IMessageBus _messageBus = messageBus;

        private readonly IGenericRepository<MedicalHistory> _medicalHistoryRepository = unitOfWork.Repository<MedicalHistory>();
        private readonly IGenericRepository<Complaint> _complaintRepository = unitOfWork.Repository<Complaint>();
        private readonly IGenericRepository<ComplaintSummary> _complaintSummaryRepository = unitOfWork.Repository<ComplaintSummary>();

        #region Complaint Methods
        public async Task<PaginationResult<GetComplaintDTO>> GetAllAsync(ComplaintFilterDTO complaintFilterDTO)
        {
            var (total, complaints) = await _complaintRepository.GetAllPaginatedAsync(
                complaintFilterDTO.Filter,
                complaintFilterDTO.Sort,
                complaintFilterDTO.Page,
                complaintFilterDTO.PageSize);

            var complaintsDTO = _mapper.Map<List<GetComplaintDTO>>(complaints);
            return new PaginationResult<GetComplaintDTO>(total, complaintFilterDTO.PageSize, complaintsDTO);
        }

        public async Task<GetComplaintDTO> CreateAsync(int medicalHistoryId, CreateComplaintDTO createComplaintDTO)
        {
            var medicalHistory = await _medicalHistoryRepository.GetByIdAsync(medicalHistoryId);
            if (medicalHistory == null)
            {
                throw new DataNotFoundException(typeof(MedicalHistory), medicalHistoryId);
            }

            if (medicalHistory.MedicalHistoryStatus != MedicalHistoryStatusEnum.Completed)
            {
                throw new InvalidDataException("Cannot create complaint for a medical history that is not completed");
            }

            if (medicalHistory.Complaint != null)
            {
                throw new InvalidDataException($"Complaint already exists for this medical history");
            }

            if (medicalHistory.PatientId != _currentUserService.UserId)
            {
                throw new UnauthorizedAccessException("You are not authorized to create a complaint for this medical history");
            }

            var complaint = _mapper.Map<Complaint>(createComplaintDTO);
            complaint.MedicalHistoryId = medicalHistoryId;

            var createdComplaint = await _complaintRepository.AddAsync(complaint);
            await _unitOfWork.SaveChangeAsync();

            await _messageBus.PublishAsync<PredictComplaintTypeEvent>(new() { ComplaintId = createdComplaint.Id });

            return _mapper.Map<GetComplaintDTO>(complaint);
        }

        public async Task UpdateStatusAsync(int complaintId, ComplaintResolveStatusEnum complaintResolveStatus, string? response = null)
        {
            var complaint = await _complaintRepository.GetByIdAsync(complaintId);
            if (complaint == null)
            {
                throw new DataNotFoundException(typeof(Complaint), complaintId);
            }

            var complaintStateMachine = new ComplaintStateMachine(complaint);
            if (!complaintStateMachine.CanFire(complaintResolveStatus))
            {
                throw new InvalidDataException($"Cannot change status from {complaint.ComplaintResolveStatus} to {complaintResolveStatus}");
            }

            try
            {
                complaintStateMachine.Fire(complaintResolveStatus, response);
                _complaintRepository.Update(complaint);
                await _unitOfWork.SaveChangeAsync();
            }
            catch (Exception e)
            {
                throw new InvalidDataException($"Failed to change complaint status: {e.Message}");
            }
        }

        public async Task ClassifyAsync(int complaintId, ComplaintTypeEnum complaintType)
        {
            var complaint = await _complaintRepository.GetByIdAsync(complaintId);
            if (complaint == null)
            {
                throw new DataNotFoundException(typeof(Complaint), complaintId);
            }

            if (complaint.ComplaintType == complaintType)
            {
                return;
            }

            complaint.ComplaintType = complaintType;
            _complaintRepository.Update(complaint);
            await _unitOfWork.SaveChangeAsync();
        }
        #endregion

        #region Complaint Summary Methods
        public async Task CreateComplaintSummaryForLastWeekAsync()
        {
            var today = DateTime.UtcNow.Date;

            // Get week number for last week
            var daysSinceMonday = ((int)today.DayOfWeek - (int)DayOfWeek.Monday + 7) % 7;
            var startOfThisWeek = today.AddDays(-daysSinceMonday);

            var startOfLastWeek = startOfThisWeek.AddDays(-7);
            var endOfLastWeek = startOfThisWeek.AddTicks(-1);

            var weekOfYear = TimeUtil.GetWeekOfYear(startOfLastWeek);
            var year = startOfLastWeek.Year;

            // Check if summary already exists
            var isLastWeekSummaryExist = await _complaintSummaryRepository
                .AnyAsync(cs => cs.Year == year && cs.WeekOfYear == weekOfYear);

            if (isLastWeekSummaryExist)
            {
                return;
            }

            // Get complaints from last week
            var complaintsLastWeek = await _complaintRepository
                .GetAllAsync(c => c.CreationDate >= startOfLastWeek && c.CreationDate <= endOfLastWeek);

            var complaintTexts = complaintsLastWeek
                .OrderBy(c => c.CreationDate)
                .Select(c => c.Content?.Trim() ?? string.Empty)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToList();

            // Get summary from LLMService
            var summaryEvent = new GetWeeklyComplaintsSummaryEvent
            {
                Complaints = complaintTexts
            };
            var summaryContract = await _messageBus.RequestAsync<
                GetWeeklyComplaintsSummaryEvent,
                GetWeeklyComplaintsSummaryContract>(summaryEvent);

            if (string.IsNullOrEmpty(summaryContract.Summary))
            {
                return;
            }

            var complaintSummary = new ComplaintSummary
            {
                Year = year,
                WeekOfYear = weekOfYear,
                Summary = summaryContract.Summary,
            };
            complaintSummary.TryValidate();

            await _complaintSummaryRepository.AddAsync(complaintSummary);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<string> GetComplaintSummaryByWeekAsync(string yearWeekString)
        {
            var parts = yearWeekString.Split("-W");
            if (parts.Length != 2 ||
                !int.TryParse(parts[0], out var year) ||
                !int.TryParse(parts[1], out var weekOfYear))
            {
                throw new InvalidDataException("Invalid year-week format. Expected format: YYYY-Ww");
            }

            var complaintSummary = await _complaintSummaryRepository.GetByConditionAsync(cs => cs.Year == year && cs.WeekOfYear == weekOfYear);
            if (complaintSummary == null)
            {
                throw new DataNotFoundException($"Complaint summary for year {year} and week {weekOfYear} not found");
            }

            return complaintSummary.Summary;
        }
        #endregion

        #region AI-Related Methods
        public async Task GetTypePredictionAsync(int complaintId)
        {
            var complaint = await _complaintRepository.GetByIdAsync(complaintId);
            if (complaint == null)
            {
                throw new DataNotFoundException(typeof(Complaint), complaintId);
            }

            var predictionTypeString = await _complaintPredictionApiService.GetComplaintTypePredictionAsync(complaint.Content);

            complaint.ComplaintType = Enum.TryParse<ComplaintTypeEnum>(predictionTypeString, out var predictionType)
                ? predictionType
                : ComplaintTypeEnum.Neutral;

            _complaintRepository.Update(complaint);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task RetrainTypePredictionModelAsync()
        {
            await _complaintPredictionApiService.RetrainComplaintTypeModelAsync();
        }
        #endregion
    }
}