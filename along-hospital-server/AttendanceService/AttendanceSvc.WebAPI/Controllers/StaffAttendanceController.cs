using AttendanceSvc.BLL.DTOs;
using AttendanceSvc.BLL.FilterDTOs;
using AttendanceSvc.BLL.Interfaces;
using AttendanceSvc.DAL.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Commons.Results;
using SharedLibrary.Commons.Settings;
using SharedLibrary.Services.Interfaces;

namespace AttendanceSvc.WebAPI.Controllers
{
    [Authorize(Roles = RolePolicies.StaffRolePolicy)]
    public class StaffAttendanceController(
        IAttendanceService attendanceService,
        ICurrentUserService currentUserService,
        IStaffRecognizationApiService staffRecognizationApiService) : BaseController
    {
        private readonly IAttendanceService _attendanceService = attendanceService;
        private readonly ICurrentUserService _currentUserService = currentUserService;
        private readonly IStaffRecognizationApiService _staffRecognizationApiService = staffRecognizationApiService;

        [HttpGet]
        public async Task<IActionResult> GetMyAttendance(AttendanceFilterDTO attendanceFilterDTO)
        {
            var staffId = _currentUserService.UserId;
            var getMyAttendance = await _attendanceService.GetAllByStaffIdPaginatedAsync(staffId, attendanceFilterDTO);
            return Result.SuccessData(getMyAttendance);
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetAttendanceStats()
        {
            var staffId = _currentUserService.UserId;
            var attendanceStats = await _attendanceService.GetAttendanceStatsAsync(staffId);
            return Result.SuccessData(attendanceStats);
        }

        [HttpGet("check-identification")]
        public async Task<IActionResult> CheckIdentificationExist()
        {
            var isIdentificationExist = await _staffRecognizationApiService.CheckIdentificationExistAsync(_currentUserService.UserId);
            return Result.SuccessData(isIdentificationExist);
        }

        [HttpPost("enroll")]
        public async Task<IActionResult> EnrollIdentification(EnrollStaffIdentificationDTO enrollStaffIdentificationDTO)
        {
            var message = await _staffRecognizationApiService.EnrollAsync(
                _currentUserService.UserId,
                enrollStaffIdentificationDTO.Images
            );
            return Result.SuccessAction(message);
        }

        [HttpDelete("reset-identification")]
        public async Task<IActionResult> ResetIdentification()
        {
            var message = await _staffRecognizationApiService.ResetIdentificationAsync(
                _currentUserService.UserId
            );
            return Result.SuccessAction(message);
        }

        [HttpPost("check-in")]
        public async Task<IActionResult> CheckIn(CreateAttendanceRequestDTO createAttendanceRequestDTO)
        {
            var currentStaffId = _currentUserService.UserId;

            var isIdentificationExist = await _staffRecognizationApiService.CheckIdentificationExistAsync(currentStaffId);
            if (!isIdentificationExist)
            {
                throw new DataNotFoundException("No identification data found for the current user. Please enroll your identification.");
            }

            var recognizedStaffId = await _staffRecognizationApiService.RecognizeStaffAsync(createAttendanceRequestDTO.File);
            if (recognizedStaffId != currentStaffId)
            {
                throw new UnauthorizedAccessException("Face does not match with the current user.");
            }

            var createDTO = new CreateAttendanceDTO
            {
                StaffId = currentStaffId,
                LogType = nameof(AttendanceLogTypeEnum.CheckIn)
            };

            await _attendanceService.CreateAsync(createDTO);
            return Result.SuccessAction("Check-in successfully");
        }

        [HttpPost("check-out")]
        public async Task<IActionResult> CheckOut(CreateAttendanceRequestDTO createAttendanceRequestDTO)
        {
            var currentStaffId = _currentUserService.UserId;

            var isIdentificationExist = await _staffRecognizationApiService.CheckIdentificationExistAsync(currentStaffId);
            if (!isIdentificationExist)
            {
                throw new DataNotFoundException("No identification data found for the current user. Please enroll your identification.");
            }

            var recognizedStaffId = await _staffRecognizationApiService.RecognizeStaffAsync(createAttendanceRequestDTO.File);
            if (recognizedStaffId != currentStaffId)
            {
                throw new UnauthorizedAccessException("Face does not match with the current user.");
            }

            var createDTO = new CreateAttendanceDTO
            {
                StaffId = currentStaffId,
                LogType = nameof(AttendanceLogTypeEnum.CheckOut)
            };

            await _attendanceService.CreateAsync(createDTO);
            return Result.SuccessAction("Check-out successfully");
        }
    }
}
