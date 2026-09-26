using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Enums;
using WorkScheduleSvc.BLL.DTOs.TemplateDTOs.WorkScheduleTemplateForStaffRoom;
using WorkScheduleSvc.BLL.DTOs.TemplateDTOs.WorkScheduleTemplateForTeleRoom;
using WorkScheduleSvc.BLL.Interfaces;

namespace WorkScheduleSvc.WebAPI.Controllers
{
    [Authorize(Roles = nameof(RoleEnum.HR))]
    public class WorkScheduleTemplateAssignmentManagementController(
        IWorkScheduleTemplateRoomAssignmentService roomAssignmentService,
        IWorkScheduleTemplateTeleRoomAssignmentService teleRoomAssignmentService)
        : BaseController
    {
        private readonly IWorkScheduleTemplateRoomAssignmentService _roomAssignmentService = roomAssignmentService;
        private readonly IWorkScheduleTemplateTeleRoomAssignmentService _teleRoomAssignmentService = teleRoomAssignmentService;

        [HttpGet("{templateId}/rooms")]
        public async Task<IActionResult> GetRoomAssignments(int templateId, [FromQuery] int? shiftId, [FromQuery] int? roomId, [FromQuery] int? staffId)
        {
            var result = await _roomAssignmentService.GetRoomAssignmentsAsync(templateId, shiftId, roomId, staffId);
            return Result.SuccessData(result);
        }

        [HttpPost("{templateId}/rooms")]
        public async Task<IActionResult> CreateRoomAssignments(int templateId, CreateWorkScheduleTemplateAssignmentForStaffRoomDTO createDTO)
        {
            await _roomAssignmentService.CreateRoomAssignmentsAsync(templateId, createDTO);
            return Result.SuccessAction("Room assignments created successfully");
        }

        [HttpDelete("{templateId}/rooms")]
        public async Task<IActionResult> DeleteRoomAssignment(int templateId, [FromQuery] int shiftId, [FromQuery] int roomId, [FromQuery] int staffId)
        {
            var deleteDTO = new DeleteWorkScheduleTemplateAssignmentForStaffRoomDTO
            {
                ShiftId = shiftId,
                RoomId = roomId,
                StaffId = staffId,
            };
            await _roomAssignmentService.DeleteRoomAssignmentAsync(templateId, deleteDTO);
            return Result.SuccessAction("Room assignment deleted successfully");
        }

        [HttpGet("{templateId}/tele-rooms")]
        public async Task<IActionResult> GetTeleRoomAssignments(int templateId, [FromQuery] int? shiftId, [FromQuery] int? teleRoomId, [FromQuery] int? staffId)
        {
            var result = await _teleRoomAssignmentService.GetTeleRoomAssignmentsAsync(templateId, shiftId, teleRoomId, staffId);
            return Result.SuccessData(result);
        }

        [HttpPost("{templateId}/tele-rooms")]
        public async Task<IActionResult> CreateTeleRoomAssignments(int templateId, CreateWorkScheduleTemplateAssignmentForStaffTeleRoomDTO createDTO)
        {
            await _teleRoomAssignmentService.CreateTeleRoomAssignmentsAsync(templateId, createDTO);
            return Result.SuccessAction("Tele room assignments created successfully");
        }

        [HttpDelete("{templateId}/tele-rooms")]
        public async Task<IActionResult> DeleteTeleRoomAssignment(int templateId, [FromQuery] int shiftId, [FromQuery] int teleRoomId, [FromQuery] int staffId)
        {
            var deleteDTO = new DeleteWorkScheduleTemplateAssignmentForStaffTeleRoomDTO
            {
                ShiftId = shiftId,
                TeleRoomId = teleRoomId,
                StaffId = staffId,
            };
            await _teleRoomAssignmentService.DeleteTeleRoomAssignmentAsync(templateId, deleteDTO);
            return Result.SuccessAction("Tele room assignment deleted successfully");
        }
    }
}
