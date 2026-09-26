using Microsoft.AspNetCore.Authorization;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Enums;
using StaffSvc.BLL.DTOs.StaffCertificateTypes;
using StaffSvc.BLL.FilterDTOs;
using StaffSvc.BLL.Interfaces;

namespace StaffSvc.WebAPI.Controllers.Management
{
    [Authorize(Roles = nameof(RoleEnum.HR))]
    public class StaffCertificateTypeManagementController(
        IStaffCertificateTypeService staffCertificateTypeService)
        : CrudController<UpsertStaffCertificateTypeDTO, UpsertStaffCertificateTypeDTO, GetStaffCertificateTypeDTO, StaffCertificateTypeFilterDTO>(
            staffCertificateTypeService)
    {
        protected override string? EntityName => "Staff Certificate Type";
    }
}