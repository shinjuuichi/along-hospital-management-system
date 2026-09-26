using MedicalHistorySvc.BLL.DTOs.ComplaintDTOs;
using MedicalHistorySvc.BLL.FilterDTOs;
using MedicalHistorySvc.BLL.Interfaces;
using MedicalHistorySvc.DAL.Enums;
using MedicalHistorySvc.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Commons.Results;
using SharedLibrary.Commons.Settings;
using SharedLibrary.Enums;
using SharedLibrary.Services.Interfaces;

namespace MedicalHistorySvc.WebAPI.Controllers
{
    [Authorize]
    public class MedicalHistoryController(
        IMedicalHistoryQueryService medicalHistoryService,
        IComplaintService complaintService,
        ICurrentUserService currentUserService)
            : BaseController
    {
        private readonly IMedicalHistoryQueryService _medicalHistoryService = medicalHistoryService;
        private readonly IComplaintService _complaintService = complaintService;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        [HttpGet]
        [Authorize(Roles = nameof(RoleEnum.Patient))]
        public async Task<IActionResult> GetAll(MedicalHistoryFilterDTO medicalHistoryFilterDTO)
        {
            var patientId = _currentUserService.UserId;
            var medicalHistoryDTOs = await _medicalHistoryService.GetAllByPatientIdAsync(patientId, medicalHistoryFilterDTO);
            return Result.SuccessData(medicalHistoryDTOs);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = $"{RolePolicies.MedicalStaffRolePolicy}," +
            $"{nameof(RoleEnum.HotlineAgent)}," +
            $"{nameof(RoleEnum.Patient)}")]
        public async Task<IActionResult> GetById(int id)
        {
            var medicalHistoryDTO = await _medicalHistoryService.GetByIdAsync(id);
            switch (_currentUserService.Role)
            {
                case RoleEnum.Patient:
                    if (medicalHistoryDTO.PatientId != _currentUserService.UserId)
                    {
                        throw new UnauthorizedAccessException("You are not authorized to access this medical history");
                    }
                    if (medicalHistoryDTO.MedicalHistoryStatus == nameof(MedicalHistoryStatusEnum.Draft))
                    {
                        throw new DataNotFoundException(typeof(MedicalHistory), id);
                    }
                    break;
            }

            return Result.SuccessData(medicalHistoryDTO);
        }

        [HttpPost("{medicalHistoryId}/complaint")]
        [Authorize(Roles = nameof(RoleEnum.Patient))]
        public async Task<IActionResult> CreateComplaint(int medicalHistoryId, CreateComplaintDTO createComplaintDTO)
        {
            var complaintDTO = await _complaintService.CreateAsync(medicalHistoryId, createComplaintDTO);
            return Result.SuccessData(complaintDTO, $"Create complaint to medical history #{medicalHistoryId} successfully");
        }
    }
}
