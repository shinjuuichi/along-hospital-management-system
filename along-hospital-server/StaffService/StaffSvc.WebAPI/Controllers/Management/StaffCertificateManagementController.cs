using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Enums;
using StaffSvc.BLL.DTOs.StaffCertificates;
using StaffSvc.BLL.FilterDTOs;
using StaffSvc.BLL.Interfaces;

namespace StaffSvc.WebAPI.Controllers.Management
{
    [Authorize(Roles = nameof(RoleEnum.HR))]
    public class StaffCertificateManagementController(
        IStaffCertificateService staffCertificateService)
        : GetController<GetStaffCertificateDTO, StaffCertificateFilterDTO>(staffCertificateService)
    {
        [HttpPost]
        public async Task<IActionResult> Create(UpsertStaffCertificateDTO createDTO)
        {
            var result = await staffCertificateService.CreateAsync(createDTO);
            return Result.SuccessData(result, "Staff Certificate created successfully");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await staffCertificateService.DeleteAsync(id);
            return Result.SuccessAction("Staff Certificate deleted successfully");
        }

        [HttpPut("suspend/{id}")]
        public async Task<IActionResult> Suspend(int id, string reason)
        {
            await staffCertificateService.SuspendCertificatesAsync(id, reason);
            return Result.SuccessAction("Certificate suspended successfully.");
        }

        [HttpPut("active/{id}")]
        public async Task<IActionResult> Active(int id, DateOnly expiredDate)
        {
            await staffCertificateService.ActiveCertificatesAsync(id, expiredDate);
            return Result.SuccessAction("Certificate actived successfully.");
        }

        [HttpPut("approve/{id}")]
        public async Task<IActionResult> Approve(int id)
        {
            await staffCertificateService.ApproveCertificatesAsync(id);
            return Result.SuccessAction("Certificate approved successfully.");
        }
    }
}