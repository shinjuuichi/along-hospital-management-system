using AppointmentSvc.BLL.FilterDTOs;
using AppointmentSvc.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Enums;

namespace AppointmentSvc.WebAPI.Controllers
{
    [Authorize(Roles = nameof(RoleEnum.Doctor))]
    public class AppointmentManagementController(IAppointmentQueryService appointmentQueryService) : BaseController
    {
        private readonly IAppointmentQueryService _appointmentQueryService = appointmentQueryService;

        [HttpGet]
        public async Task<IActionResult> GetAllTelehealthByCurrentDoctorSpecialty(AppointmentFilterDTO appointmentFilterDTO)
        {
            var appointmentDTOs = await _appointmentQueryService.GetAllTelehealthPaginatedByCurrentDoctorSpecialtyAsync(appointmentFilterDTO);
            return Result.SuccessData(appointmentDTOs);
        }
    }
}