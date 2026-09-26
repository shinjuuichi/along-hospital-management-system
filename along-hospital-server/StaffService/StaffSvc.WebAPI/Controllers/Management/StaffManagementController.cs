using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Enums;
using StaffSvc.BLL.DTOs.StaffAccountDTOs;
using StaffSvc.BLL.FilterDTOs;
using StaffSvc.BLL.Interfaces;
using StaffSvc.DAL.Enums;

namespace StaffSvc.WebAPI.Controllers.Management
{
    [Authorize(Roles = nameof(RoleEnum.HR))]
    public class StaffManagementController(
        IStaffService staffService)
        : GetController<GetStaffAndAccountDTO, StaffFilterDTO>(staffService)
    {
        private readonly IStaffService _staffService = staffService;

        [AllowAnonymous]
        public override Task<IActionResult> GetAll()
        {
            return base.GetAll();
        }

        [HttpPost("staff-account")]
        public async Task<IActionResult> CreateStaffAndAccountAsync(CreateStaffAndAccountDTO createStaffAndAccount)
        {
            await _staffService.CreateStaffAndAccountAsync(createStaffAndAccount);
            return Result.SuccessAction("Staff and account created successfully.");
        }

        [HttpPut("staff-account/{staffId}")]
        public async Task<IActionResult> UpdateStaffAndAccountAsync(int staffId, UpdateStaffAndAccountDTO updateStaffAndAccount)
        {
            await _staffService.UpdateStaffAndAccountAsync(staffId, updateStaffAndAccount);
            return Result.SuccessAction("Staff and account updated successfully.");
        }

        [HttpPut("terminate")]
        public async Task<IActionResult> TerminateAsync([FromQuery] List<int> ids)
        {
            await _staffService.UpdateStatusAsync(ids, StaffStatusEnum.Terminated);
            return Result.SuccessAction("Staff status updated to terminated successfully.");
        }

        [HttpPut("onleave")]
        public async Task<IActionResult> OnLeaveAsync([FromQuery] List<int> ids)
        {
            await _staffService.UpdateStatusAsync(ids, StaffStatusEnum.OnLeave);
            return Result.SuccessAction("Staff status updated to on-leave successfully.");
        }

        [HttpPut("activate")]
        public async Task<IActionResult> ActivateAsync([FromQuery] List<int> ids)
        {
            await _staffService.UpdateStatusAsync(ids, StaffStatusEnum.Active);
            return Result.SuccessAction("Staff status updated to active successfully.");
        }
    }
}
