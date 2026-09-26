using Microsoft.AspNetCore.Authorization;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Enums;
using StaffSvc.BLL.DTOs.StaffGroupDTOs;
using StaffSvc.BLL.FilterDTOs;
using StaffSvc.BLL.Interfaces;

namespace StaffSvc.WebAPI.Controllers.Management
{
    [Authorize(Roles = nameof(RoleEnum.HR))]
    public class StaffGroupManagementController(IStaffGroupService staffGroupService)
        : CrudController<
            UpsertStaffGroupDTO,
            UpsertStaffGroupDTO,
            GetStaffGroupDTO,
            StaffGroupFilterDTO>(staffGroupService)
    {
        protected override string? EntityName => "Staff Group";
    }
}