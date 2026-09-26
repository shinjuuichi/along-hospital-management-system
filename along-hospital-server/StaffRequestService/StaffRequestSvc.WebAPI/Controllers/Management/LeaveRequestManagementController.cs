using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Commons.Settings;
using StaffRequestSvc.BLL.DTOs.LeaveRequestDTOs;
using StaffRequestSvc.BLL.FilterDTOs;
using StaffRequestSvc.BLL.Interfaces;
using StaffRequestSvc.DAL.Enums;

namespace StaffRequestSvc.WebAPI.Controllers.Management
{
    [Authorize(Roles = RolePolicies.StaffRequestRolePolicy)]
    public class LeaveRequestManagementController(ILeaveRequestService _leaveRequestService)
    : GetController<GetLeaveRequestDTO, LeaveRequestFilterDTO>(_leaveRequestService)
    {
        [HttpPut("approve/{id}")]
        public async Task<IActionResult> Approve(int id)
        {
            await _leaveRequestService.ChangeStatusAsync(id, RequestStatusEnum.Approved);
            return Result.SuccessAction("Leave request approved successfully");
        }

        [HttpPut("reject/{id}")]
        public async Task<IActionResult> Reject(int id)
        {
            await _leaveRequestService.ChangeStatusAsync(id, RequestStatusEnum.Rejected);
            return Result.SuccessAction("Leave request rejected successfully");
        }
    }
}