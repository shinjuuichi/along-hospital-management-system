using AutoMapper;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Base.Services;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Commons.Filters;
using SharedLibrary.Commons.Results;
using WorkScheduleSvc.BLL.DTOs;
using WorkScheduleSvc.BLL.DTOs.WorkScheduleDTOs;
using WorkScheduleSvc.BLL.DTOs.WorkSegmentDTOs;
using WorkScheduleSvc.BLL.Interfaces;
using WorkScheduleSvc.BLL.Interfaces.Calculators;
using WorkScheduleSvc.BLL.Interfaces.Gateways;
using WorkScheduleSvc.BLL.Interfaces.Querys;
using WorkScheduleSvc.BLL.Interfaces.Validators;
using WorkScheduleSvc.BLL.StateMachines;
using WorkScheduleSvc.DAL.Enums;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.BLL.Implements
{
    public class WorkScheduleService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IWorkScheduleValidator workScheduleValidator,
        IWorkScheduleQueryService workScheduleQueryService,
        IWorkSegmentCalculator workSegmentCalculator,
        IWorkSegmentGateway workSegmentGateway)
        : BaseService<WorkSchedule, CreateWorkScheduleDTO, UpdateWorkScheduleDTO, GetWorkScheduleDTO>(
            unitOfWork,
            mapper,
            includes: [nameof(WorkSchedule.Shift)]),
        IWorkScheduleService
    {
        private const string LeaveUnitDay = "Day";
        private const string LeaveUnitShift = "Shift";

        private readonly IWorkScheduleValidator _workScheduleValidator = workScheduleValidator;
        private readonly IWorkScheduleQueryService _workScheduleQueryService = workScheduleQueryService;
        private readonly IWorkSegmentCalculator _workSegmentCalculator = workSegmentCalculator;
        private readonly IWorkSegmentGateway _workSegmentGateway = workSegmentGateway;

        private readonly IGenericRepository<WorkScheduleAssignment> _workScheduleAssignmentRepository = unitOfWork.Repository<WorkScheduleAssignment>();
        private readonly IGenericRepository<WorkSegment> _workSegmentRepository = unitOfWork.Repository<WorkSegment>();
        private readonly IGenericRepository<Shift> _shiftRepository = unitOfWork.Repository<Shift>();

        #region Override Methods
        public override async Task<List<GetWorkScheduleDTO>> GetAllAsync()
        {
            var workSchedules = await _repository.GetAllAsync(null, _includes);
            return await _workScheduleQueryService.BuildWorkScheduleDTOsAsync(workSchedules);
        }

        public override async Task<GetWorkScheduleDTO> GetByIdAsync(int id)
        {
            var workSchedule = await _repository.GetByIdAsync(id, _includes)
                ?? throw new DataNotFoundException(typeof(WorkSchedule), id);

            return await _workScheduleQueryService.BuildWorkScheduleDTOAsync(workSchedule);
        }

        public override async Task<PaginationResult<GetWorkScheduleDTO>> GetAllPaginatedAsync(FilterDTO filterDTO)
        {
            var (total, workSchedules) = await _repository.GetAllPaginatedAsync(
                filterDTO.Filter,
                filterDTO.Sort,
                filterDTO.Page,
                filterDTO.PageSize,
                _includes);

            var workScheduleDTOs = await _workScheduleQueryService.BuildWorkScheduleDTOsAsync(workSchedules);
            return new PaginationResult<GetWorkScheduleDTO>(total, filterDTO.PageSize, workScheduleDTOs);
        }

        public override async Task<GetWorkScheduleDTO> UpdateAsync(int id, UpdateWorkScheduleDTO updateDTO)
        {
            var existing = await _repository.GetByIdAsync(id)
                ?? throw new DataNotFoundException(typeof(WorkSchedule), id);
            _workScheduleValidator.ValidateWorkScheduleMutableStatus(existing);

            await _workScheduleValidator.ValidateWorkScheduleAsync(updateDTO.WorkDate, updateDTO.ShiftId, excludeId: id);

            var result = await base.UpdateAsync(id, updateDTO);
            if (existing.WorkScheduleTemplateId.HasValue)
            {

                var workScheduleAssignments = await _workScheduleQueryService.GetWorkScheduleAssignmentsAsync(id);
                var templateAssignments = await _workScheduleQueryService.BuildTemplateAssignmentsAsync(
                    id,
                    existing.WorkScheduleTemplateId,
                    updateDTO.ShiftId);

                if (workScheduleAssignments.Count > 0)
                {
                    _workScheduleAssignmentRepository.RemoveRange(workScheduleAssignments);
                }

                if (templateAssignments.Count > 0)
                {
                    await _workScheduleAssignmentRepository.AddRangeAsync(templateAssignments);
                }

                if (workScheduleAssignments.Count > 0 || templateAssignments.Count > 0)
                {
                    await _unitOfWork.SaveChangeAsync();
                }
            }

            return result;
        }

        public override async Task DeleteAsync(int id)
        {
            var workSchedule = await _repository.GetByIdAsync(id)
                ?? throw new DataNotFoundException(typeof(WorkSchedule), id);
            _workScheduleValidator.ValidateWorkScheduleMutableStatus(workSchedule);

            _repository.Remove(workSchedule);
            await _unitOfWork.SaveChangeAsync();
        }
        #endregion

        #region Primary Methods
        public async Task<List<GetWorkScheduleDTO>> GenerateAsync(CreateWorkScheduleDTO createDTO)
        {
            _workScheduleValidator.ValidateGenerateMode(createDTO);
            _workScheduleValidator.ValidateDateRange(createDTO.FromDate, createDTO.ToDate);

            var generatedIds = new List<int>();
            var templateDayShiftDTOs = new List<(DayOfWeekEnum DayOfWeek, int ShiftId)>();

            if (createDTO.WorkScheduleTemplateId.HasValue)
            {
                await _workScheduleValidator.ValidateTemplateActiveAsync(createDTO.WorkScheduleTemplateId.Value);
                templateDayShiftDTOs = await _workScheduleQueryService.GetTemplateDayShiftsAsync(createDTO.WorkScheduleTemplateId.Value);

                if (templateDayShiftDTOs.Count == 0)
                {
                    return [];
                }
            }
            else
            {
                await _workScheduleValidator.ValidateShiftIdAsync(createDTO.ShiftId!.Value);
            }

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                for (var date = createDTO.FromDate; date <= createDTO.ToDate; date = date.AddDays(1))
                {
                    if (await _workScheduleValidator.IsHolidayAsync(date))
                    {
                        continue;
                    }

                    var shiftIds = createDTO.WorkScheduleTemplateId.HasValue
                        ? templateDayShiftDTOs
                            .Where(x => x.DayOfWeek == (DayOfWeekEnum)date.DayOfWeek)
                            .Select(x => x.ShiftId)
                            .Distinct()
                            .ToList()
                        : [createDTO.ShiftId!.Value];

                    if (shiftIds.Count == 0)
                    {
                        continue;
                    }

                    var newSchedules = new List<WorkSchedule>();
                    foreach (var shiftId in shiftIds)
                    {
                        await _workScheduleValidator.ValidateWorkScheduleAsync(date, shiftId);
                        newSchedules.Add(new WorkSchedule
                        {
                            WorkDate = date,
                            ShiftId = shiftId,
                            WorkScheduleTemplateId = createDTO.WorkScheduleTemplateId,
                            WorkScheduleStatus = WorkScheduleStatusEnum.Draft
                        });
                    }

                    if (newSchedules.Count == 0)
                    {
                        continue;
                    }

                    await _repository.AddRangeAsync(newSchedules);
                    await _unitOfWork.SaveChangeAsync();

                    var assignmentRepository = _unitOfWork.Repository<WorkScheduleAssignment>();
                    foreach (var workSchedule in newSchedules)
                    {
                        var templateAssignments = await _workScheduleQueryService.BuildTemplateAssignmentsAsync(
                            workSchedule.Id,
                            createDTO.WorkScheduleTemplateId,
                            workSchedule.ShiftId);

                        if (templateAssignments.Count > 0)
                        {
                            await assignmentRepository.AddRangeAsync(templateAssignments);
                            await _unitOfWork.SaveChangeAsync();
                        }

                        generatedIds.Add(workSchedule.Id);
                    }
                }

                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }

            if (generatedIds.Count == 0)
            {
                return [];
            }

            var workSchedules = await _repository.GetAllByIdsAsync(generatedIds, _includes);
            return await _workScheduleQueryService.BuildWorkScheduleDTOsAsync(workSchedules);
        }

        public async Task<List<GetWorkScheduleDTO>> GetWorkScheduleForStaffAsync(GetWorkScheduleRangeDTO rangeDTO)
        {
            _workScheduleValidator.ValidateWorkScheduleRange(rangeDTO);
            return await _workScheduleQueryService.GetWorkSchedulesForCurrentStaffAsync(rangeDTO);
        }

        public async Task<List<GetWorkScheduleDTO>> GetWorkScheduleForDateRangeAsync(GetWorkScheduleRangeDTO rangeDTO)
        {
            _workScheduleValidator.ValidateWorkScheduleRange(rangeDTO);
            return await _workScheduleQueryService.GetWorkSchedulesForDateRangeAsync(rangeDTO);
        }

        public async Task<List<GetStaffDTO>> GetAllCurrentWorkingDoctorsAsync(int? specialtyId)
        {
            return await _workScheduleQueryService.GetAllCurrentWorkingDoctorsBySpecialtyAsync(specialtyId);
        }

        public async Task<List<GetWorkScheduleDTO>> GetListWorkScheduleByWorkDateAsync(DateOnly workDate)
        {
            var workSchedules = await _repository.GetAllAsync(x =>
                x.WorkDate == workDate
                && (x.WorkScheduleStatus == WorkScheduleStatusEnum.Locked
                    || x.WorkScheduleStatus == WorkScheduleStatusEnum.Finalized), _includes);
            return await _workScheduleQueryService.BuildWorkScheduleDTOsAsync(workSchedules);
        }

        public async Task<List<GetWorkScheduleDTO>> UpdateStatusRangeAsync(UpdateWorkScheduleStatusRangeDTO dto, WorkScheduleStatusEnum newStatus)
        {
            _workScheduleValidator.ValidateWorkScheduleIds(dto.WorkScheduleIds);

            var workScheduleIds = dto.WorkScheduleIds.Distinct().ToList();
            var workSchedules = await _repository.GetAllByIdsAsync(workScheduleIds);
            _workScheduleValidator.ValidateWorkSchedulesFound(workSchedules);
            if (workSchedules.Count != workScheduleIds.Count)
            {
                var foundIds = workSchedules.Select(x => x.Id).ToHashSet();
                var firstNotFoundId = workScheduleIds.First(id => !foundIds.Contains(id));
                throw new DataNotFoundException(typeof(WorkSchedule), firstNotFoundId);
            }

            foreach (var workSchedule in workSchedules)
            {
                if (workSchedule.WorkScheduleStatus == newStatus)
                {
                    continue;
                }

                _workScheduleValidator.ValidateStatusTransition(workSchedule, newStatus);
                var stateMachine = new WorkScheduleStateMachine(workSchedule);
                stateMachine.Fire(newStatus);
            }

            _repository.UpdateRange(workSchedules);
            await _unitOfWork.SaveChangeAsync();

            return await _workScheduleQueryService.BuildWorkScheduleDTOsAsync(workSchedules);
        }

        public async Task<List<GetWorkScheduleDTO>> FinalizeRangeAsync(UpdateWorkScheduleStatusRangeDTO dto)
        {
            _workScheduleValidator.ValidateWorkScheduleIds(dto.WorkScheduleIds);

            var workScheduleIds = dto.WorkScheduleIds.Distinct().ToList();
            var workSchedules = await _repository.GetAllByIdsAsync(workScheduleIds);
            _workScheduleValidator.ValidateWorkSchedulesFound(workSchedules);
            if (workSchedules.Count != workScheduleIds.Count)
            {
                var foundIds = workSchedules.Select(x => x.Id).ToHashSet();
                var firstNotFoundId = workScheduleIds.First(id => !foundIds.Contains(id));
                throw new DataNotFoundException(typeof(WorkSchedule), firstNotFoundId);
            }

            await this.ValidateWorkSchedulesHaveWorkSegmentsAsync(workScheduleIds);

            foreach (var workSchedule in workSchedules)
            {
                if (workSchedule.WorkScheduleStatus == WorkScheduleStatusEnum.Finalized)
                {
                    continue;
                }

                _workScheduleValidator.ValidateStatusTransition(workSchedule, WorkScheduleStatusEnum.Finalized);
                var stateMachine = new WorkScheduleStateMachine(workSchedule);
                stateMachine.Fire(WorkScheduleStatusEnum.Finalized);
            }

            _repository.UpdateRange(workSchedules);
            await _unitOfWork.SaveChangeAsync();

            workScheduleIds = workSchedules.Select(x => x.Id).ToList();
            var workSegments = await _workSegmentRepository.GetAllAsync(
                x => x.WorkScheduleAssignment != null && workScheduleIds.Contains(x.WorkScheduleAssignment.WorkScheduleId),
                [nameof(WorkSegment.WorkScheduleAssignment)]);

            var payrollByStaff = new Dictionary<int, WorkSegmentPayrollSummaryDTO>();
            var assignmentGroups = workSegments
                .Where(x => x.WorkScheduleAssignment != null)
                .GroupBy(x => x.WorkScheduleAssignmentId)
                .ToList();

            foreach (var assignmentGroup in assignmentGroups)
            {
                var assignment = assignmentGroup.First().WorkScheduleAssignment!;
                var payrollSummaryDTO = _workSegmentCalculator.CalculatePayrollSummary(assignmentGroup.ToList());

                if (!payrollByStaff.TryGetValue(assignment.StaffId, out var staffPayrollDTO))
                {
                    staffPayrollDTO = new WorkSegmentPayrollSummaryDTO();
                }

                staffPayrollDTO.TotalWorkedMinutes += payrollSummaryDTO.TotalWorkedMinutes;
                staffPayrollDTO.OvertimeMinutes += payrollSummaryDTO.OvertimeMinutes;
                staffPayrollDTO.LateMinutes += payrollSummaryDTO.LateMinutes;
                staffPayrollDTO.EarlyLeaveMinutes += payrollSummaryDTO.EarlyLeaveMinutes;

                payrollByStaff[assignment.StaffId] = staffPayrollDTO;
            }

            foreach (var payrollByStaffItem in payrollByStaff)
            {
                await _workSegmentGateway.PublishPayrollAsync(payrollByStaffItem.Key, payrollByStaffItem.Value);
            }

            return await _workScheduleQueryService.BuildWorkScheduleDTOsAsync(workSchedules);
        }

        public async Task ValidateLeaveRequestAsync(LeaveRequestValidationDTO leaveRequestValidationDTO)
        {
            ArgumentNullException.ThrowIfNull(leaveRequestValidationDTO);

            if (string.IsNullOrWhiteSpace(leaveRequestValidationDTO.LeaveUnit))
            {
                throw new ValidationFailureException(
                    nameof(LeaveRequestValidationDTO.LeaveUnit),
                    "Leave unit is required");
            }

            if (string.Equals(leaveRequestValidationDTO.LeaveUnit, LeaveUnitDay, StringComparison.OrdinalIgnoreCase))
            {
                await this.ValidateDayLeaveAsync(
                    leaveRequestValidationDTO.StaffId,
                    leaveRequestValidationDTO.FromDate,
                    leaveRequestValidationDTO.ToDate);
                return;
            }

            if (string.Equals(leaveRequestValidationDTO.LeaveUnit, LeaveUnitShift, StringComparison.OrdinalIgnoreCase))
            {
                await this.ValidateShiftLeaveAsync(
                    leaveRequestValidationDTO.StaffId,
                    leaveRequestValidationDTO.FromDate,
                    leaveRequestValidationDTO.ShiftId);
                return;
            }

            throw new ValidationFailureException(
                nameof(LeaveRequestValidationDTO.LeaveUnit),
                $"Leave unit must be {LeaveUnitDay} or {LeaveUnitShift}");
        }

        public async Task HandleApprovedLeaveRequestAsync(LeaveRequestValidationDTO leaveRequestValidationDTO)
        {
            if (string.Equals(leaveRequestValidationDTO.LeaveUnit, LeaveUnitDay, StringComparison.OrdinalIgnoreCase))
            {
                await this.RemoveAssignmentsForDayLeaveAsync(
                    leaveRequestValidationDTO.StaffId,
                    leaveRequestValidationDTO.FromDate,
                    leaveRequestValidationDTO.ToDate);
                return;
            }

            if (string.Equals(leaveRequestValidationDTO.LeaveUnit, LeaveUnitShift, StringComparison.OrdinalIgnoreCase))
            {
                await this.RemoveAssignmentsForShiftLeaveAsync(
                    leaveRequestValidationDTO.StaffId,
                    leaveRequestValidationDTO.FromDate,
                    leaveRequestValidationDTO.ShiftId);
                return;
            }
        }
        #endregion

        #region Helper Methods
        private async Task ValidateDayLeaveAsync(int staffId, DateOnly fromDate, DateOnly toDate)
        {
            var assignments = await _workScheduleAssignmentRepository.GetAllAsync(
                x => x.StaffId == staffId
                    && x.WorkSchedule != null
                    && (x.WorkSchedule.WorkScheduleStatus == WorkScheduleStatusEnum.Published)
                    && x.WorkSchedule.WorkDate >= fromDate
                    && x.WorkSchedule.WorkDate <= toDate,
                [nameof(WorkScheduleAssignment.WorkSchedule)]);

            var availableDates = assignments
                .Where(x => x.WorkSchedule != null)
                .Select(x => x.WorkSchedule!.WorkDate)
                .Distinct()
                .ToHashSet();

            for (var workDate = fromDate; workDate <= toDate; workDate = workDate.AddDays(1))
            {
                if (!availableDates.Contains(workDate))
                {
                    throw new ValidationFailureException(
                        nameof(LeaveRequestValidationDTO.FromDate),
                        $"Staff does not have a work schedule on {workDate:yyyy-MM-dd}");
                }
            }
        }

        private async Task ValidateShiftLeaveAsync(int staffId, DateOnly fromDate, int? shiftId)
        {
            if (!shiftId.HasValue)
            {
                throw new ValidationFailureException(
                    nameof(LeaveRequestValidationDTO.ShiftId),
                    "Shift is required for shift leave");
            }

            var shift = await _shiftRepository.GetByIdAsync(shiftId.Value)
                ?? throw new DataNotFoundException(typeof(Shift), shiftId.Value);

            var assignment = await _workScheduleAssignmentRepository.GetByConditionAsync(
                x => x.StaffId == staffId
                    && x.WorkSchedule != null
                    && x.WorkSchedule.WorkDate == fromDate
                    && x.WorkSchedule.ShiftId == shiftId.Value
                    && (x.WorkSchedule.WorkScheduleStatus == WorkScheduleStatusEnum.Published),
                [nameof(WorkScheduleAssignment.WorkSchedule)]);

            if (assignment is null)
            {
                throw new ValidationFailureException(
                    nameof(LeaveRequestValidationDTO.ShiftId),
                    $"Staff does not have shift '{shift.Name}' on {fromDate:yyyy-MM-dd}");
            }
        }

        private async Task RemoveAssignmentsForDayLeaveAsync(int staffId, DateOnly fromDate, DateOnly toDate)
        {
            var assignments = await _workScheduleAssignmentRepository.GetAllAsync(
                x => x.StaffId == staffId
                    && x.WorkSchedule != null
                    && (x.WorkSchedule.WorkScheduleStatus == WorkScheduleStatusEnum.Published)
                    && x.WorkSchedule.WorkDate >= fromDate
                    && x.WorkSchedule.WorkDate <= toDate);

            if (assignments.Count == 0)
            {
                return;
            }

            _workScheduleAssignmentRepository.RemoveRange(assignments);
            await _unitOfWork.SaveChangeAsync();
        }

        private async Task RemoveAssignmentsForShiftLeaveAsync(int staffId, DateOnly workDate, int? shiftId)
        {
            if (!shiftId.HasValue)
            {
                throw new ValidationFailureException(
                    nameof(LeaveRequestValidationDTO.ShiftId),
                    "Shift is required for shift leave");
            }

            var assignments = await _workScheduleAssignmentRepository.GetAllAsync(
                x => x.StaffId == staffId
                    && x.WorkSchedule != null
                    && (x.WorkSchedule.WorkScheduleStatus == WorkScheduleStatusEnum.Published)
                    && x.WorkSchedule.WorkDate == workDate
                    && x.WorkSchedule.ShiftId == shiftId.Value);

            if (assignments.Count == 0)
            {
                return;
            }

            _workScheduleAssignmentRepository.RemoveRange(assignments);
            await _unitOfWork.SaveChangeAsync();
        }
        #endregion

        private async Task ValidateWorkSchedulesHaveWorkSegmentsAsync(List<int> workScheduleIds)
        {
            var finalizedScheduleIdsWithSegments = (await _workSegmentRepository.GetAllAsync(
                x => x.WorkScheduleAssignment != null
                    && workScheduleIds.Contains(x.WorkScheduleAssignment.WorkScheduleId),
                [nameof(WorkSegment.WorkScheduleAssignment)]))
                .Where(x => x.WorkScheduleAssignment != null)
                .Select(x => x.WorkScheduleAssignment!.WorkScheduleId)
                .Distinct()
                .ToHashSet();

            var invalidWorkScheduleIds = workScheduleIds
                .Where(x => !finalizedScheduleIdsWithSegments.Contains(x))
                .ToList();

            if (invalidWorkScheduleIds.Count > 0)
            {
                throw new ValidationFailureException(
                    nameof(UpdateWorkScheduleStatusRangeDTO.WorkScheduleIds),
                    $"WorkSchedule [{string.Join(", ", invalidWorkScheduleIds)}] must have at least one WorkSegment before finalized.");
            }
        }
    }
}
