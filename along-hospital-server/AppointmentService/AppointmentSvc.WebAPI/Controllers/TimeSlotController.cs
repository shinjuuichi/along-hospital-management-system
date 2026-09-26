using AppointmentSvc.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Enums;

namespace AppointmentSvc.WebAPI.Controllers
{
    [Authorize(Roles = nameof(RoleEnum.Patient))]
    public class TimeSlotController(ITimeSlotService timeSlotService) : BaseController
    {
        private readonly ITimeSlotService _timeSlotService = timeSlotService;

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableTimeSlots(DateOnly date, int specialtyId)
        {
            var timeSlots = await _timeSlotService.GetAvailableTimeSlotsAsync(date, specialtyId);
            return Result.SuccessData(timeSlots);
        }
    }
}