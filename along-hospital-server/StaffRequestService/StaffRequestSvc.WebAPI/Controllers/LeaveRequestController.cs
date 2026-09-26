using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Commons.Settings;
using StaffRequestSvc.BLL.DTOs.LeaveRequestDTOs;
using StaffRequestSvc.BLL.FilterDTOs;
using StaffRequestSvc.BLL.Interfaces;

namespace StaffRequestSvc.WebAPI.Controllers
{
    [Authorize(Roles = RolePolicies.StaffRolePolicy)]
    public class LeaveRequestController(ILeaveRequestService _leaveRequestService)
        : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateLeaveRequestDTO createDTO)
        {
            await _leaveRequestService.CreateAsync(createDTO);
            return Result.SuccessAction("Leave request created successfully");
        }

        [HttpGet]
        public async Task<IActionResult> GetMyRequests(LeaveRequestFilterDTO filterDTO)
        {
            var result = await _leaveRequestService.GetMyRequestsAsync(filterDTO);
            return Result.SuccessData(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _leaveRequestService.GetByIdAsync(id);
            return Result.SuccessData(result);
        }

        [HttpPut("cancel/{id}")]
        public async Task<IActionResult> Cancel(int id)
        {
            await _leaveRequestService.ChangeStatusAsync(id, StaffRequestSvc.DAL.Enums.RequestStatusEnum.Canceled);
            return Result.SuccessAction("Leave request canceled successfully");
        }
    }
}