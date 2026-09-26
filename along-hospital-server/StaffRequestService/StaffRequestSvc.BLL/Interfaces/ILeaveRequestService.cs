using SharedLibrary.Base.Services;
using SharedLibrary.Commons.Results;
using StaffRequestSvc.BLL.DTOs;
using StaffRequestSvc.BLL.DTOs.LeaveRequestDTOs;
using StaffRequestSvc.BLL.FilterDTOs;
using StaffRequestSvc.DAL.Enums;

namespace StaffRequestSvc.BLL.Interfaces
{
    public interface ILeaveRequestService : IBaseCrudService<CreateLeaveRequestDTO, UpdateLeaveRequestDTO, GetLeaveRequestDTO>
    {
        Task<PaginationResult<GetLeaveRequestDTO>> GetMyRequestsAsync(LeaveRequestFilterDTO filterDTO);
        Task<GetLeaveRequestDTO> ChangeStatusAsync(int id, RequestStatusEnum action);
        Task<List<ApprovedLeaveDTO>> GetApprovedLeavesByStaffsAndRangeAsync(GetApprovedLeavesByStaffsRangeDTO approvedLeavesByStaffsRangeDTO);
    }
}