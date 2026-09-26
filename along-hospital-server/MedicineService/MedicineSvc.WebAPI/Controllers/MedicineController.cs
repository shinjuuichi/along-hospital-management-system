using MedicineSvc.BLL.DTOs.FilterDTOs;
using MedicineSvc.BLL.DTOs.MedicineDTOs;
using MedicineSvc.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;

namespace MedicineSvc.WebAPI.Controllers
{
    public class MedicineController(IMedicineService medicineService)
        : GetController<GetMedicineDTO, MedicineFilterDTO>(medicineService)
    {
        [HttpGet("infusion/all")]
        public async Task<IActionResult> GetAllInfusion()
        {
            var result = await medicineService.GetAllInfusionMedicinesAsync();
            return Result.SuccessData(result);
        }
    }
}