using MedicalServiceSvc.BLL.DTOs;
using MedicalServiceSvc.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Enums;

namespace MedicalServiceSvc.WebAPI.Controllers
{
    [Authorize(Roles = nameof(RoleEnum.Manager))]
    public class MedicalServiceManagementController(IMedicalServiceService medicalServiceService)
        : CrudController<UpsertMedicalServiceDTO, UpsertMedicalServiceDTO, GetMedicalServiceDTO, MedicalServiceFilterDTO>(medicalServiceService)
    {
        protected override string? EntityName => "Medical Service";

        [AllowAnonymous]
        public override async Task<IActionResult> GetAll()
        {
            var result = await medicalServiceService.GetAllForCurrentUserAsync();
            return Result.SuccessData(result);
        }
    }
}
