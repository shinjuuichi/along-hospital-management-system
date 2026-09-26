using AppointmentSvc.BLL.DTOs;
using AppointmentSvc.BLL.FilterDTOs;
using AppointmentSvc.BLL.Interfaces;
using AppointmentSvc.DAL.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Enums;
using SharedLibrary.Services.Interfaces;

namespace AppointmentSvc.WebAPI.Controllers
{
    [Authorize(Roles = nameof(RoleEnum.Patient))]
    public class AppointmentController(
        IAppointmentQueryService appointmentQueryService,
        IAppointmentCommandService appointmentCommandService,
        ICurrentUserService currentUserService)
            : BaseController
    {
        private readonly IAppointmentQueryService _appointmentQueryService = appointmentQueryService;
        private readonly IAppointmentCommandService _appointmentCommandService = appointmentCommandService;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        [HttpGet]
        public async Task<IActionResult> GetAllByUserId([FromQuery] AppointmentFilterDTO appointmentFilterDTO)
        {
            var userId = _currentUserService.UserId;
            var appointmentDTOs = await _appointmentQueryService.GetAllPaginatedByPatientIdAsync(userId, appointmentFilterDTO);
            return Result.SuccessData(appointmentDTOs);
        }

        [HttpGet("{id}/payment-url")]
        public async Task<IActionResult> GetPaymentUrl(int id)
        {
            var paymentUrl = await _appointmentQueryService.GetPaymentUrlByAppointmentIdAsync(id);
            return Result.SuccessData(paymentUrl);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAppointmentRequestDTO createAppointmentDTO)
        {
            var responseDTO = await _appointmentCommandService.CreateAsync(createAppointmentDTO);
            return Result.SuccessData(responseDTO, "Appointment created successfully");
        }

        [HttpPut("cancel/{appointmentId}")]
        public async Task<IActionResult> Cancel(int appointmentId)
        {
            await _appointmentCommandService.UpdateStatusAsync(appointmentId, AppointmentStatusEnum.Cancelled);
            return Result.SuccessAction("Mark appointment as cancelled successfully");
        }
    }
}