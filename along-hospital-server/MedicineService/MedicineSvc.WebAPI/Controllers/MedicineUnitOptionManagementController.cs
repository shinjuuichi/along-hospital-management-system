using MedicineSvc.BLL.DTOs.MedicineUnitOptionDTOs;
using MedicineSvc.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;

namespace MedicineSvc.WebAPI.Controllers
{
    public class MedicineUnitOptionManagementController(IMedicineUnitOptionService service)
        : BaseController
    {
        [HttpPut]
        public async Task<IActionResult> UpdateStatus(UpdateMedicineUnitOptionDTO updateDto)
        {
            await service.UpdateStatusAsync(updateDto);
            return Result.SuccessAction("Medicine Unit Option updated successfully");
        }
    }
}