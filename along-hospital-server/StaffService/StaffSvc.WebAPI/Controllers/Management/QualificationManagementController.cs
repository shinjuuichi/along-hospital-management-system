using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Enums;
using StaffSvc.BLL.DTOs.QualificationDTOs;
using StaffSvc.BLL.FilterDTOs;
using StaffSvc.BLL.Interfaces;

namespace StaffSvc.WebAPI.Controllers.Management
{
    [Authorize(Roles = nameof(RoleEnum.Manager))]
    public class QualificationManagementController(IQualificationService qualificationService)
        : CrudController<CreateQualificationDTO, UpdateQualificationDTO, GetQualificationDTO, QualificationFilterDTO>(
            qualificationService)
    {
        protected override string EntityName => "Qualification";

        [AllowAnonymous]
        public override Task<IActionResult> GetAll()
        {
            return base.GetAll();
        }
    }
}