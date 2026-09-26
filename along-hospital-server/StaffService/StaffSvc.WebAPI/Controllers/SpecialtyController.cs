using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using StaffSvc.BLL.DTOs.SpecialtyDTOs;
using StaffSvc.BLL.Interfaces;

namespace StaffSvc.WebAPI.Controllers
{
    public class SpecialtyController(ISpecialtyService specialtyService)
        : GetController<GetSpecialtyDTO>(specialtyService)
    {
        private readonly ISpecialtyService _specialtyService = specialtyService;

        public override async Task<IActionResult> GetAll()
        {
            var specialties = await _specialtyService.GetMedicalSpecialtiesAsync();
            return Result.SuccessData(specialties);
        }
    }
}