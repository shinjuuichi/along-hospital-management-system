using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Enums;
using SupplierSvc.BLL.DTOs.ImportRequestDTOs;
using SupplierSvc.BLL.FilterDTOs;
using SupplierSvc.BLL.Interfaces;
using SupplierSvc.DAL.Enums;

namespace SupplierSvc.WebAPI.Controllers
{
    [Authorize]
    public class ImportRequestManagementController(IImportRequestService importRequestService)
        : CrudController<CreateImportRequestDTO, UpdateImportRequestDTO, GetImportRequestDTO, ImportRequestFilterDTO>(importRequestService)
    {
        private readonly IImportRequestService _importRequestService = importRequestService;
        protected override string EntityName => "ImportRequest";

        [Authorize(Roles = $"{nameof(RoleEnum.InventoryClerk)},{nameof(RoleEnum.Pharmacist)}")]
        public override async Task<IActionResult> Create(CreateImportRequestDTO createDTO)
        {
            return await base.Create(createDTO);
        }

        [Authorize(Roles = $"{nameof(RoleEnum.InventoryClerk)},{nameof(RoleEnum.Pharmacist)}")]
        public override async Task<IActionResult> Update(int id, UpdateImportRequestDTO updateDTO)
        {
            return await base.Update(id, updateDTO);
        }

        [Authorize(Roles = nameof(RoleEnum.Pharmacist))]
        [HttpPut("approve/{id:int}")]
        public async Task<IActionResult> Approve(int id)
        {
            await _importRequestService.ChangeStatusAsync(id, ImportRequestStatusEnum.Approved);
            return Result.SuccessAction($"ImportRequest #{id} approved successfully");
        }

        [Authorize(Roles = nameof(RoleEnum.Pharmacist))]
        [HttpPut("reject/{id:int}")]
        public async Task<IActionResult> Reject(int id)
        {
            await _importRequestService.ChangeStatusAsync(id, ImportRequestStatusEnum.Rejected);
            return Result.SuccessAction($"ImportRequest #{id} rejected successfully");
        }

        [Authorize(Roles = nameof(RoleEnum.InventoryClerk))]
        [HttpPut("cancel/{id:int}")]
        public async Task<IActionResult> Cancel(int id)
        {
            await _importRequestService.ChangeStatusAsync(id, ImportRequestStatusEnum.Cancelled);
            return Result.SuccessAction($"ImportRequest #{id} cancelled successfully");
        }
    }
}