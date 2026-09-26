using AutoMapper;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Services;
using SharedLibrary.Commons.Exceptions;
using WorkScheduleSvc.BLL.DTOs.WorkSegmentDTOs;
using WorkScheduleSvc.BLL.Interfaces;
using WorkScheduleSvc.BLL.Interfaces.Calculators;
using WorkScheduleSvc.BLL.Interfaces.Gateways;
using WorkScheduleSvc.BLL.Interfaces.Querys;
using WorkScheduleSvc.BLL.Interfaces.Validators;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.BLL.Implements
{
    public class WorkSegmentService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IWorkSegmentValidator workSegmentValidator,
        IWorkSegmentGateway workSegmentGateway,
        IWorkSegmentCalculator workSegmentCalculator,
        IWorkSegmentQueryService workSegmentQueryService)
        : BaseService<WorkSegment, CreateWorkSegmentDTO, UpdateWorkSegmentDTO, GetWorkSegmentDTO>(
            unitOfWork,
            mapper),
        IWorkSegmentService
    {
        private readonly IWorkSegmentValidator _workSegmentValidator = workSegmentValidator;
        private readonly IWorkSegmentGateway _workSegmentGateway = workSegmentGateway;
        private readonly IWorkSegmentCalculator _workSegmentCalculator = workSegmentCalculator;
        private readonly IWorkSegmentQueryService _workSegmentQueryService = workSegmentQueryService;

        #region Override Methods
        public override async Task<GetWorkSegmentDTO> UpdateAsync(int id, UpdateWorkSegmentDTO dto)
        {
            var existing = await _repository.GetByIdAsync(
                id,
                [
                    nameof(WorkSegment.WorkScheduleAssignment),
                    $"{nameof(WorkSegment.WorkScheduleAssignment)}.{nameof(WorkScheduleAssignment.WorkSchedule)}"
                ])
                    ?? throw new DataNotFoundException(typeof(WorkSegment), id);

            var workSchedule = existing.WorkScheduleAssignment?.WorkSchedule
                ?? throw new DataNotFoundException("Work schedule was not found for work segment.");

            _workSegmentValidator.ValidateMutableStatus(workSchedule, "updated");

            _repository.Update(existing);
            await _unitOfWork.SaveChangeAsync();
            return _mapper.Map<GetWorkSegmentDTO>(existing);
        }
        #endregion

        #region Primary Methods
        public async Task<List<GetWorkSegmentDTO>> GetByAssignmentIdsAsync(List<int> assignmentIds)
        {
            if (assignmentIds.Count == 0)
            {
                return [];
            }

            var workSegments = await _repository.GetAllAsync(x => assignmentIds.Contains(x.WorkScheduleAssignmentId));
            return _mapper.Map<List<GetWorkSegmentDTO>>(workSegments);
        }

        public async Task GenerateAsync(DateTime periodStart, DateTime periodEnd)
        {
            var periodDTO = _workSegmentQueryService.NormalizePeriod(periodStart, periodEnd);
            _workSegmentValidator.ValidatePeriod(periodDTO);
            var assignments = await _workSegmentQueryService.GetAssignmentsForGenerationAsync(periodDTO);

            if (assignments.Count == 0)
            {
                return;
            }

            var assignmentIds = assignments.Select(x => x.Id).ToList();
            var existingSegments = await _repository.GetAllAsync(
                x => assignmentIds.Contains(x.WorkScheduleAssignmentId)
                     && x.StartTime >= periodDTO.PeriodStart
                     && x.StartTime < periodDTO.PeriodEnd);

            if (existingSegments.Count > 0)
            {
                _repository.RemoveRange(existingSegments);
            }

            var staffIds = assignments.Select(x => x.StaffId).Distinct().ToList();
            var attendanceByStaff = await _workSegmentGateway.GetAttendanceLogsAsync(staffIds, periodDTO);
            var newSegments = new List<WorkSegment>();

            foreach (var assignment in assignments)
            {
                var attendanceContracts = attendanceByStaff.TryGetValue(assignment.StaffId, out var attendanceLogs)
                    ? attendanceLogs
                    : [];

                var workSegments = _workSegmentCalculator.BuildSegments(
                    assignment,
                    attendanceContracts);
                newSegments.AddRange(workSegments);
            }

            if (newSegments.Count > 0)
            {
                await _repository.AddRangeAsync(newSegments);
            }

            await _unitOfWork.SaveChangeAsync();
        }
        #endregion
    }
}
