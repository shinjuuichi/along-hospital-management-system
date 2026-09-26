using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Commons.Exceptions;
using WorkScheduleSvc.BLL.DTOs.WorkScheduleDTOs;
using WorkScheduleSvc.BLL.Interfaces.Validators;
using WorkScheduleSvc.BLL.StateMachines;
using WorkScheduleSvc.DAL.Enums;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.BLL.Implements.Validators
{
    public class WorkScheduleValidator(IUnitOfWork unitOfWork) : IWorkScheduleValidator
    {
        private readonly IGenericRepository<Holiday> _holidayRepository = unitOfWork.Repository<Holiday>();
        private readonly IGenericRepository<WorkScheduleTemplate> _templateRepository = unitOfWork.Repository<WorkScheduleTemplate>();
        private readonly IGenericRepository<WorkSchedule> _workScheduleRepository = unitOfWork.Repository<WorkSchedule>();
        private readonly IGenericRepository<Shift> _shiftRepository = unitOfWork.Repository<Shift>();

        public void ValidateDateRange(DateOnly fromDate, DateOnly toDate)
        {
            if (fromDate > toDate)
            {
                throw new InvalidDataException("FromDate must be before or equal to ToDate.");
            }
        }

        public void ValidateGenerateMode(CreateWorkScheduleDTO dto)
        {
            var hasTemplate = dto.WorkScheduleTemplateId.HasValue;
            var hasShift = dto.ShiftId.HasValue;

            if (hasTemplate == hasShift)
            {
                throw new ValidationFailureException(
                    nameof(CreateWorkScheduleDTO.WorkScheduleTemplateId),
                    "Provide exactly one of WorkScheduleTemplateId or ShiftId.");
            }
        }

        public async Task ValidateShiftIdAsync(int shiftId)
        {
            var isExists = await _shiftRepository.AnyAsync(x => x.Id == shiftId);
            if (!isExists)
            {
                throw new DataNotFoundException(typeof(Shift), shiftId);
            }
        }

        public async Task ValidateTemplateActiveAsync(int templateId)
        {
            var template = await _templateRepository.GetByIdAsync(templateId)
                ?? throw new DataNotFoundException(typeof(WorkScheduleTemplate), templateId);

            if (!template.IsActive)
            {
                throw new ValidationFailureException(nameof(CreateWorkScheduleDTO.WorkScheduleTemplateId), $"WorkScheduleTemplate with id {templateId} is not active.");
            }
        }

        public void ValidateWorkScheduleRange(GetWorkScheduleRangeDTO rangeDTO)
        {
            if (rangeDTO.FromDate.HasValue != rangeDTO.ToDate.HasValue)
            {
                throw new ValidationFailureException("DateRange", "FromDate and ToDate must be provided together.");
            }

            if (rangeDTO.FromDate.HasValue && rangeDTO.ToDate.HasValue)
            {
                this.ValidateDateRange(rangeDTO.FromDate.Value, rangeDTO.ToDate.Value);
            }
        }

        public void ValidateWorkScheduleIds(List<int> workScheduleIds)
        {
            if (workScheduleIds.Count == 0)
            {
                throw new ValidationFailureException(nameof(UpdateWorkScheduleStatusRangeDTO.WorkScheduleIds), "WorkScheduleIds is required.");
            }
        }

        public void ValidateWorkSchedulesFound(List<WorkSchedule> workSchedules)
        {
            if (workSchedules.Count == 0)
            {
                throw new DataNotFoundException("No work schedule found.");
            }
        }

        public async Task ValidateWorkScheduleAsync(DateOnly workDate, int shiftId, int? excludeId = null)
        {
            var isDuplicate = await _workScheduleRepository.AnyAsync(x =>
                x.WorkDate == workDate
                && x.ShiftId == shiftId
                && (!excludeId.HasValue || x.Id != excludeId.Value));

            if (isDuplicate)
            {
                throw new DataConflictException("Work schedule already exists for the specified date and shift.");
            }
        }

        public void ValidateWorkScheduleMutableStatus(WorkSchedule workSchedule)
        {
            if (workSchedule.WorkScheduleStatus != WorkScheduleStatusEnum.Draft
                && workSchedule.WorkScheduleStatus != WorkScheduleStatusEnum.Published)
            {
                throw new ValidationFailureException(
                    nameof(WorkSchedule.WorkScheduleStatus),
                    "WorkSchedule can only be changed when status is Draft or Published.");
            }
        }

        public void ValidateStatusTransition(WorkSchedule workSchedule, WorkScheduleStatusEnum newStatus)
        {
            var stateMachine = new WorkScheduleStateMachine(workSchedule);
            if (!stateMachine.CanFire(newStatus))
            {
                throw new ValidationFailureException(
                    "Status",
                    $"Cannot transition from {workSchedule.WorkScheduleStatus} to {newStatus}.");
            }
        }

        #region Helper Methods
        public async Task<bool> IsHolidayAsync(DateOnly date)
        {
            return await _holidayRepository.AnyAsync(x =>
                x.Day == date.Day
                && x.Month == date.Month
                && (!x.Year.HasValue || x.Year == date.Year));
        }

        #endregion
    }
}
