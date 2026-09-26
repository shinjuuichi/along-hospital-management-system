using SharedLibrary.Base.Services;
using WorkScheduleSvc.BLL.DTOs;
using WorkScheduleSvc.BLL.DTOs.WorkScheduleDTOs;
using WorkScheduleSvc.DAL.Enums;

namespace WorkScheduleSvc.BLL.Interfaces
{
    public interface IWorkScheduleService : IBaseCrudService<CreateWorkScheduleDTO, UpdateWorkScheduleDTO, GetWorkScheduleDTO>
    {
        Task<List<GetWorkScheduleDTO>> GenerateAsync(CreateWorkScheduleDTO createDTO);
        Task<List<GetWorkScheduleDTO>> GetWorkScheduleForDateRangeAsync(GetWorkScheduleRangeDTO rangeDTO);
        Task<List<GetWorkScheduleDTO>> GetWorkScheduleForStaffAsync(GetWorkScheduleRangeDTO rangeDTO);
        Task<List<GetStaffDTO>> GetAllCurrentWorkingDoctorsAsync(int? specialtyId);
        Task<List<GetWorkScheduleDTO>> GetListWorkScheduleByWorkDateAsync(DateOnly workDate);
        Task<List<GetWorkScheduleDTO>> UpdateStatusRangeAsync(UpdateWorkScheduleStatusRangeDTO dto, WorkScheduleStatusEnum newStatus);
        Task<List<GetWorkScheduleDTO>> FinalizeRangeAsync(UpdateWorkScheduleStatusRangeDTO dto);
        Task ValidateLeaveRequestAsync(LeaveRequestValidationDTO leaveRequestValidationDTO);
        Task HandleApprovedLeaveRequestAsync(LeaveRequestValidationDTO leaveRequestValidationDTO);
    }
}
