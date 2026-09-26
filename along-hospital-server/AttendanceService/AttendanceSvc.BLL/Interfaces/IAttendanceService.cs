using AttendanceSvc.BLL.DTOs;
using AttendanceSvc.BLL.FilterDTOs;
using SharedLibrary.Base.Services;
using SharedLibrary.Commons.Results;

namespace AttendanceSvc.BLL.Interfaces
{
    public interface IAttendanceService
        : IBaseCrudService<CreateAttendanceDTO, CreateAttendanceDTO, GetAttendanceDTO>
    {
        Task<PaginationResult<GetAttendanceDTO>> GetAllByStaffIdPaginatedAsync(int staffId, AttendanceFilterDTO filterDTO);
        Task<List<GetAttendanceLogDTO>> GetAttendanceLogsByStaffsAndRangeAsync(GetAttendanceLogsByStaffsRangeDTO attendanceLogsByStaffsRangeDTO);
        Task<GetAttendanceStatsDTO> GetAttendanceStatsAsync(int staffId);
    }
}