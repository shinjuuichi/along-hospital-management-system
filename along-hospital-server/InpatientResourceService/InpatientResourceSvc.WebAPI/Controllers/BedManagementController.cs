using InpatientResourceSvc.BLL.DTOs.BedDTOs;
using InpatientResourceSvc.BLL.FilterDTOs;
using InpatientResourceSvc.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Settings;

namespace InpatientResourceSvc.WebAPI.Controllers
{
    [Authorize(Roles = RolePolicies.MedicalStaffRolePolicy)]
    public class BedManagementController(IBedService bedService)
        : CrudController<UpsertBedDTO, UpsertBedDTO, GetBedDTO, BedFilterDTO>(bedService)
    {
        protected override string? EntityName => "Bed";
    }
}