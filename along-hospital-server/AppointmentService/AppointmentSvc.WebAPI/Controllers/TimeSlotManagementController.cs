using AppointmentSvc.BLL.DTOs.TimeSlotDTOs;
using AppointmentSvc.BLL.FilterDTOs;
using AppointmentSvc.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Enums;

namespace AppointmentSvc.WebAPI.Controllers
{
    [Authorize(Roles = nameof(RoleEnum.Receptionist))]
    public class TimeSlotManagementController(ITimeSlotService timeSlotService)
        : CrudController<UpsertTimeSlotDTO, UpsertTimeSlotDTO, GetTimeSlotDTO, TimeSlotFilterDTO>(timeSlotService)
    {
        protected override string? EntityName => "Time Slot";
    }
}