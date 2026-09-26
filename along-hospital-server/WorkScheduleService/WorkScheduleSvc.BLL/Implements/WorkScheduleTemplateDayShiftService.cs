using AutoMapper;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Commons.Exceptions;
using WorkScheduleSvc.BLL.DTOs.TemplateDTOs.WorkScheduleTemplateDayShiftDTOs;
using WorkScheduleSvc.BLL.Interfaces;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.BLL.Implements
{
    public class WorkScheduleTemplateDayShiftService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : IWorkScheduleTemplateDayShiftService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IGenericRepository<WorkScheduleTemplate> _templateRepository = unitOfWork.Repository<WorkScheduleTemplate>();
        private readonly IGenericRepository<Shift> _shiftRepository = unitOfWork.Repository<Shift>();
        private readonly IGenericRepository<WorkScheduleTemplateDayShift> _dayShiftRepository = unitOfWork.Repository<WorkScheduleTemplateDayShift>();
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<List<GetWorkScheduleTemplateDayShiftDTO>> GetDayShiftsAsync(int templateId)
        {
            var template = await this.GetTemplateOrThrowAsync(templateId,
                [nameof(WorkScheduleTemplate.WorkScheduleTemplateDayShifts)]);

            return _mapper.Map<List<GetWorkScheduleTemplateDayShiftDTO>>(template.WorkScheduleTemplateDayShifts);
        }

        public async Task UpdateDayShiftsAsync(
            int templateId,
            List<UpdateWorkScheduleTemplateDayShiftDTO> dayShifts)
        {
            var template = await this.GetTemplateOrThrowAsync(templateId,
                [nameof(WorkScheduleTemplate.WorkScheduleTemplateDayShifts)]);

            var normalized = dayShifts
                .GroupBy(x => new { x.DayOfWeek, x.ShiftId })
                .Select(x => x.First())
                .ToList();

            var shiftIds = normalized.Select(x => x.ShiftId).Distinct().ToList();
            if (shiftIds.Count > 0)
            {
                var existingShiftCount = await _shiftRepository.CountAsync(x => shiftIds.Contains(x.Id));
                if (existingShiftCount != shiftIds.Count)
                {
                    throw new DataNotFoundException("One or more shifts do not exist.");
                }

                await this.ValidateNoShiftTimeConflictsByDayAsync(normalized);
            }

            var currentDayShifts = template.WorkScheduleTemplateDayShifts.ToList();
            if (currentDayShifts.Count > 0)
            {
                _dayShiftRepository.RemoveRange(currentDayShifts);
            }

            foreach (var ds in normalized)
            {
                template.WorkScheduleTemplateDayShifts.Add(new WorkScheduleTemplateDayShift
                {
                    WorkScheduleTemplateId = templateId,
                    DayOfWeek = ds.DayOfWeek,
                    ShiftId = ds.ShiftId,
                });
            }

            await _unitOfWork.SaveChangeAsync();
        }

        #region Helper methods
        private async Task ValidateNoShiftTimeConflictsByDayAsync(List<UpdateWorkScheduleTemplateDayShiftDTO> normalized)
        {
            var shiftIds = normalized.Select(x => x.ShiftId).Distinct().ToList();
            if (shiftIds.Count <= 1)
            {
                return;
            }

            var shifts = await _shiftRepository.GetAllAsync(x => shiftIds.Contains(x.Id));
            var shiftMap = shifts.ToDictionary(x => x.Id, x => x);

            var conflictMessages = new List<string>();
            foreach (var dayGroup in normalized.GroupBy(x => x.DayOfWeek))
            {
                var dayShiftModels = dayGroup
                    .Where(x => shiftMap.ContainsKey(x.ShiftId))
                    .Select(x => shiftMap[x.ShiftId])
                    .DistinctBy(x => x.Id)
                    .ToList();

                for (var i = 0; i < dayShiftModels.Count; i++)
                {
                    for (var j = i + 1; j < dayShiftModels.Count; j++)
                    {
                        if (!AreShiftsOverlapping(dayShiftModels[i], dayShiftModels[j]))
                        {
                            continue;
                        }

                        var firstShift = dayShiftModels[i];
                        var secondShift = dayShiftModels[j];
                        conflictMessages.Add(
                            $"{dayGroup.Key}: {BuildShiftLabel(firstShift)} overlaps {BuildShiftLabel(secondShift)}");
                    }
                }
            }

            if (conflictMessages.Count > 0)
            {
                throw new ValidationFailureException("DayShifts", string.Join("; ", conflictMessages));
            }
        }

        private static bool AreShiftsOverlapping(Shift firstShift, Shift secondShift)
        {
            var firstIntervals = ToIntervals(firstShift.StartTime, firstShift.EndTime);
            var secondIntervals = ToIntervals(secondShift.StartTime, secondShift.EndTime);

            foreach (var first in firstIntervals)
            {
                foreach (var second in secondIntervals)
                {
                    if (AreIntervalsOverlapping(first.Start, first.End, second.Start, second.End))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static List<(int Start, int End)> ToIntervals(TimeOnly startTime, TimeOnly endTime)
        {
            var startMinutes = (startTime.Hour * 60) + startTime.Minute;
            var endMinutes = (endTime.Hour * 60) + endTime.Minute;

            if (startMinutes == endMinutes)
            {
                return [(0, 24 * 60)];
            }

            if (startMinutes < endMinutes)
            {
                return [(startMinutes, endMinutes)];
            }

            return
            [
                (startMinutes, 24 * 60),
                (0, endMinutes),
            ];
        }

        private static bool AreIntervalsOverlapping(int firstStart, int firstEnd, int secondStart, int secondEnd)
        {
            return firstStart < secondEnd && secondStart < firstEnd;
        }

        private static string BuildShiftLabel(Shift shift)
        {
            return $"{shift.Name} ({shift.StartTime:HH\\:mm}-{shift.EndTime:HH\\:mm})";
        }

        private async Task<WorkScheduleTemplate> GetTemplateOrThrowAsync(int templateId, string[]? includes = null)
        {
            return await _templateRepository.GetByIdAsync(templateId, includes: includes)
                ?? throw new DataNotFoundException(typeof(WorkScheduleTemplate), templateId);
        }
        #endregion
    }
}
