using AutoMapper;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Services;
using SharedLibrary.Commons.Exceptions;
using WorkScheduleSvc.BLL.DTOs.HolidayDTOs;
using WorkScheduleSvc.BLL.Interfaces;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.BLL.Implements
{
    public class HolidayService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : BaseService<Holiday, UpsertHolidayDTO, UpsertHolidayDTO, GetHolidayDTO>(unitOfWork, mapper),
            IHolidayService
    {
        public override async Task<GetHolidayDTO> CreateAsync(UpsertHolidayDTO createDTO)
        {
            await this.ThrowIfDuplicateAsync(createDTO.Day, createDTO.Month, createDTO.Year);
            return await base.CreateAsync(createDTO);
        }

        public override async Task<GetHolidayDTO> UpdateAsync(int id, UpsertHolidayDTO updateDTO)
        {
            await this.ThrowIfDuplicateAsync(updateDTO.Day, updateDTO.Month, updateDTO.Year, excludeId: id);
            return await base.UpdateAsync(id, updateDTO);
        }

        public async Task<List<GetHolidayDTO>> CreateRangeAsync(CreateHolidayRangeDTO dto)
        {
            const int maxRangeDays = 366;

            if (dto.FromDate > dto.ToDate)
            {
                throw new ValidationFailureException("FromDate must be before or equal to ToDate.");
            }

            var rangeDays = dto.ToDate.DayNumber - dto.FromDate.DayNumber + 1;
            if (rangeDays > maxRangeDays)
            {
                throw new ValidationFailureException($"Date range cannot exceed {maxRangeDays} days.");
            }

            var rangeDates = Enumerable
                .Range(0, rangeDays)
                .Select(i => dto.FromDate.AddDays(i))
                .ToList();

            var yearsInRange = rangeDates.Select(d => d.Year).Distinct().ToList();
            var monthsInRange = rangeDates.Select(d => d.Month).Distinct().ToList();
            var daysInRange = rangeDates.Select(d => d.Day).Distinct().ToList();

            var existing = await _repository.GetAllAsync(h =>
                monthsInRange.Contains(h.Month) && daysInRange.Contains(h.Day));

            var existingSet = existing
                .Select(h => (h.Day, h.Month, h.Year))
                .ToHashSet();

            var holidays = new List<Holiday>();
            foreach (var date in rangeDates)
            {
                var isDuplicate = existingSet.Any(e =>
                    e.Day == date.Day && e.Month == date.Month
                    && (e.Year == null || e.Year == date.Year));

                if (isDuplicate)
                {
                    throw new DataConflictException(
                        $"A holiday on {date.Day}/{date.Month}/{date.Year} already exists.");
                }

                var dayDTO = _mapper.Map<UpsertHolidayDTO>(dto);
                _mapper.Map(date, dayDTO);
                holidays.Add(_mapper.Map<Holiday>(dayDTO));
            }

            await _repository.AddRangeAsync(holidays);
            await _unitOfWork.SaveChangeAsync();

            return _mapper.Map<List<GetHolidayDTO>>(holidays);
        }

        private async Task ThrowIfDuplicateAsync(int day, int month, int? year, int? excludeId = null)
        {
            var exists = await _repository.AnyAsync(h =>
                h.Day == day && h.Month == month
                && (h.Year == null || year == null || h.Year == year)
                && (excludeId == null || h.Id != excludeId));

            if (exists)
            {
                throw new DataConflictException(
                    $"A holiday on {day}/{month}{(year.HasValue ? $"/{year}" : " (recurring)")} already exists.");
            }
        }
    }
}
