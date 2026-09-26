using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Enums;
using StaffSvc.BLL.DTOs.StaffContractDTOs;
using StaffSvc.BLL.FilterDTOs;
using StaffSvc.BLL.Interfaces;
using StaffSvc.DAL.Enums;

namespace StaffSvc.WebAPI.Controllers.Management
{
    [Authorize]
    public class StaffContractManagementController(
        IStaffContractService staffContractService)
        : CrudController<CreateStaffContractDTO, UpdateStaffContractDTO, GetStaffContractDTO, StaffContractFilterDTO>(
            staffContractService)
    {
        protected override string EntityName => "StaffContract";
        private readonly IStaffContractService _staffContractService = staffContractService;

        [Authorize(Roles = nameof(RoleEnum.HR))]
        public override async Task<IActionResult> Create(CreateStaffContractDTO createDTO)
        {
            return await base.Create(createDTO);
        }

        [Authorize(Roles = nameof(RoleEnum.HR))]
        public override async Task<IActionResult> Update(int id, UpdateStaffContractDTO updateDTO)
        {
            return await base.Update(id, updateDTO);
        }

        [Authorize(Roles = nameof(RoleEnum.HR))]
        public override async Task<IActionResult> Delete(int id)
        {
            return await base.Delete(id);
        }

        [Authorize(Roles = nameof(RoleEnum.HR))]
        public override async Task<IActionResult> DeleteSelectedIds(List<int> ids)
        {
            return await base.DeleteSelectedIds(ids);
        }

        [Authorize(Roles = nameof(RoleEnum.HR))]
        [HttpPut("terminate/{id}")]
        public async Task<IActionResult> Terminate(int id)
        {
            await _staffContractService.UpdateStatusAsync(id, StaffContractStatusEnum.Terminated);
            return Result.SuccessAction("Contract terminated successfully.");
        }

        [Authorize(Roles = nameof(RoleEnum.HR))]
        [HttpPut("renew/{id}")]
        public async Task<IActionResult> Renew(int id, RenewStaffContractDTO renewDTO)
        {
            await _staffContractService.RenewAsync(id, renewDTO);
            return Result.SuccessAction("Contract renewed successfully.");
        }

        [Authorize(Roles = nameof(RoleEnum.Manager))]
        [HttpPut("sign/{id}")]
        public async Task<IActionResult> SignContract(int id, SignStaffContractDTO signDTO)
        {
            var result = await _staffContractService.SignContractAsync(id, signDTO);
            return Result.SuccessData(result, "Contract signed successfully.");
        }
    }
}
