using AttendanceSvc.BLL.DTOs;
using AttendanceSvc.BLL.FilterDTOs;
using AttendanceSvc.BLL.Interfaces;
using AttendanceSvc.DAL.Enums;
using AttendanceSvc.DAL.Models;
using AutoMapper;
using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Events.AuthAccountEvents.GetUserDataEvents;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Base.Services;
using SharedLibrary.Commons.Filters;
using SharedLibrary.Commons.Results;

namespace AttendanceSvc.BLL.Implements
{
    public class AttendanceService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMessageBus messageBus)
        : BaseService<Attendance, CreateAttendanceDTO, CreateAttendanceDTO, GetAttendanceDTO>(
            unitOfWork,
            mapper),
                IAttendanceService
    {
        private readonly IMessageBus _messageBus = messageBus;

        public override async Task<List<GetAttendanceDTO>> GetAllAsync()
        {
            var attendanceDTOs = await base.GetAllAsync();
            await this.RequestValueForAttendanceDTOsAsync(attendanceDTOs);
            return attendanceDTOs;
        }

        public override async Task<PaginationResult<GetAttendanceDTO>> GetAllPaginatedAsync(FilterDTO filterDTO)
        {
            var pageResult = await base.GetAllPaginatedAsync(filterDTO);
            await this.RequestValueForAttendanceDTOsAsync(pageResult.Collection);
            return pageResult;
        }

        public async Task<PaginationResult<GetAttendanceDTO>> GetAllByStaffIdPaginatedAsync(int staffId, AttendanceFilterDTO filterDTO)
        {
            var (total, attendances) = await _repository.GetAllPaginatedAsync(
                att => att.StaffId == staffId,
                filterDTO.Filter,
                filterDTO.Sort,
                filterDTO.Page,
                filterDTO.PageSize,
                _includes);

            var attendanceDTOs = _mapper.Map<List<GetAttendanceDTO>>(attendances);
            await this.RequestValueForAttendanceDTOsAsync(attendanceDTOs);
            return new PaginationResult<GetAttendanceDTO>(total, filterDTO.PageSize, attendanceDTOs);
        }

        public override async Task<GetAttendanceDTO> GetByIdAsync(int id)
        {
            var attendanceDTO = await base.GetByIdAsync(id);
            await this.RequestValueForAttendanceDTOsAsync([attendanceDTO]);
            return attendanceDTO;
        }

        public async Task<List<GetAttendanceLogDTO>> GetAttendanceLogsByStaffsAndRangeAsync(GetAttendanceLogsByStaffsRangeDTO attendanceLogsByStaffsRangeDTO)
        {
            if (attendanceLogsByStaffsRangeDTO.StaffIds.Count == 0)
            {
                return [];
            }

            var attendanceLogs = await _repository.GetAllAsync(
                att => attendanceLogsByStaffsRangeDTO.StaffIds.Contains(att.StaffId) &&
                       att.LogTime >= attendanceLogsByStaffsRangeDTO.FromDate &&
                       att.LogTime < attendanceLogsByStaffsRangeDTO.ToDate);

            var attendanceLogDTOs = _mapper.Map<List<GetAttendanceLogDTO>>(attendanceLogs);
            return attendanceLogDTOs
                .OrderBy(x => x.StaffId)
                .ThenBy(x => x.LogTime)
                .ToList();
        }

        public async Task<GetAttendanceStatsDTO> GetAttendanceStatsAsync(int staffId)
        {
            var todayAttendances = await _repository.GetAllAsync(att =>
                                att.StaffId == staffId &&
                                att.LogTime.Date == DateTime.UtcNow.Date);

            var todayCheckInCount = todayAttendances.Count(att => att.LogType == AttendanceLogTypeEnum.CheckIn);
            var todayCheckOutCount = todayAttendances.Count(att => att.LogType == AttendanceLogTypeEnum.CheckOut);

            var canCheckIn = (todayCheckInCount + todayCheckOutCount == 0) || todayCheckInCount <= todayCheckOutCount;
            var canCheckOut = todayCheckInCount > todayCheckOutCount;

            return new GetAttendanceStatsDTO
            {
                CanCheckIn = canCheckIn,
                CanCheckOut = canCheckOut
            };
        }

        private async Task RequestValueForAttendanceDTOsAsync(List<GetAttendanceDTO> attendanceDTOs)
        {
            var distinctStaffIds = attendanceDTOs
                .Where(a => a.StaffId != 0)
                .Select(a => a.StaffId)
                .Distinct()
                .ToList();

            if (distinctStaffIds.Count == 0)
            {
                return;
            }

            var getListStaffDataByIdsEvent = new GetListStaffDataByUserIdsEvent { UserIds = distinctStaffIds };
            var getListStaffDataByIdsContract = await _messageBus
                .RequestAsync<GetListStaffDataByUserIdsEvent, GetListStaffDataByUserIdsContract>(
                    getListStaffDataByIdsEvent);

            var staffDataDictionary = getListStaffDataByIdsContract.Data
                .ToDictionary(s => s.UserId);

            foreach (var attendanceDTO in attendanceDTOs)
            {
                if (staffDataDictionary.TryGetValue(attendanceDTO.StaffId, out var staffData))
                {
                    attendanceDTO.Staff = _mapper.Map<GetAttendanceStaffDTO>(staffData);
                }
            }
        }
    }
}