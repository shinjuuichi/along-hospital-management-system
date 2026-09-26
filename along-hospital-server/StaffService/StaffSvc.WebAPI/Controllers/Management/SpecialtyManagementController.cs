using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Enums;
using StaffSvc.BLL.DTOs.SpecialtyDTOs;
using StaffSvc.BLL.FilterDTOs;
using StaffSvc.BLL.Interfaces;

namespace StaffSvc.WebAPI.Controllers.Management
{
    [Authorize(Roles = nameof(RoleEnum.Manager))]
    public class SpecialtyManagementController(ISpecialtyService specialtyService)
        : CrudController<CreateSpecialtyDTO, UpdateSpecialtyDTO, GetSpecialtyDTO, SpecialtyFilterDTO>(specialtyService)
    {
        protected override string EntityName => "Specialty";

        [AllowAnonymous]
        public override Task<IActionResult> GetAll()
        {
            return base.GetAll();
        }
    }
}