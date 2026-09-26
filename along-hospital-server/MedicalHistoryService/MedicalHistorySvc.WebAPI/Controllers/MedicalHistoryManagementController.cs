using MedicalHistorySvc.BLL.DTOs.MedicalHistoryDTOs;
using MedicalHistorySvc.BLL.DTOs.PrescriptionDTOs.UpsertDTOs;
using MedicalHistorySvc.BLL.FilterDTOs;
using MedicalHistorySvc.BLL.Interfaces;
using MedicalHistorySvc.DAL.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Commons.Settings;
using SharedLibrary.Enums;
using SharedLibrary.Services.Interfaces;

namespace MedicalHistorySvc.WebAPI.Controllers
{
    [Authorize]
    public class MedicalHistoryManagementController(
        IMedicalHistoryQueryService medicalHistoryQueryService,
        IMedicalHistoryCommandService medicalHistoryCommandService,
        IPrescriptionService prescriptionService,
        ICurrentUserService currentUserService)
            : BaseController
    {
        private readonly IMedicalHistoryQueryService _medicalHistoryQueryService = medicalHistoryQueryService;
        private readonly IMedicalHistoryCommandService _medicalHistoryCommandService = medicalHistoryCommandService;
        private readonly IPrescriptionService _prescriptionService = prescriptionService;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        #region Get All
        [HttpGet]
        [Authorize(Roles = RolePolicies.MedicalStaffRolePolicy)]
        public async Task<IActionResult> GetAll(MedicalHistoryFilterDTO medicalHistoryFilterDTO)
        {
            var medicalHistoryDTOs = await _medicalHistoryQueryService.GetAllAsync(medicalHistoryFilterDTO);
            return Result.SuccessData(medicalHistoryDTOs);
        }

        [HttpGet("doctor")]
        [Authorize(Roles = nameof(RoleEnum.Doctor))]
        public async Task<IActionResult> GetAllByDoctor(MedicalHistoryFilterDTO medicalHistoryFilterDTO)
        {
            var doctorId = _currentUserService.UserId;
            var medicalHistoryDTOs = await _medicalHistoryQueryService.GetAllByDoctorIdAsync(doctorId, medicalHistoryFilterDTO);
            return Result.SuccessData(medicalHistoryDTOs);
        }

        [HttpGet("all/pending")]
        [Authorize(Roles = nameof(RoleEnum.Receptionist))]
        public async Task<IActionResult> GetAllPending()
        {
            var medicalHistoryDTOs = await _medicalHistoryQueryService.GetAllPendingAsync();
            return Result.SuccessData(medicalHistoryDTOs);
        }
        #endregion

        #region Create & Update
        [HttpPost]
        [Authorize(Roles = $"{nameof(RoleEnum.Receptionist)}, {nameof(RoleEnum.Nurse)}")]
        public async Task<IActionResult> Create(CreateMedicalHistoryDTO createMedicalHistoryDTO)
        {
            var medicalHistoryDTO = await _medicalHistoryCommandService.CreateAsync(createMedicalHistoryDTO);
            return Result.SuccessData(medicalHistoryDTO, "Create medical history successfully");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = nameof(RoleEnum.Doctor))]
        public async Task<IActionResult> Update(int id, UpdateMedicalHistoryDTO updateMedicalHistoryDTO)
        {
            await _medicalHistoryCommandService.UpdateAsync(id, updateMedicalHistoryDTO);
            return Result.SuccessAction("Update medical history successfully");
        }
        #endregion

        #region Prescription Operations
        [HttpPost("{medicalHistoryId}/prescription")]
        [Authorize(Roles = nameof(RoleEnum.Doctor))]
        public async Task<IActionResult> CreatePrescription(int medicalHistoryId, UpsertPrescriptionDTO upsertPrescriptionDTO)
        {
            var prescriptionDTO = await _prescriptionService.CreateAsync(medicalHistoryId, upsertPrescriptionDTO);
            return Result.SuccessData(prescriptionDTO, "Add prescription successfully");
        }

        [HttpPut("{medicalHistoryId}/prescription")]
        [Authorize(Roles = nameof(RoleEnum.Doctor))]
        public async Task<IActionResult> UpdatePrescription(int medicalHistoryId, UpsertPrescriptionDTO upsertPrescriptionDTO)
        {
            var prescriptionDTO = await _prescriptionService.UpdateByMedicalHistoryIdAsync(medicalHistoryId, upsertPrescriptionDTO);
            return Result.SuccessData(prescriptionDTO, "Update prescription successfully");
        }
        #endregion

        #region Action Operations
        [HttpPut("complete/{id}")]
        [Authorize(Roles = nameof(RoleEnum.Doctor))]
        public async Task<IActionResult> CompleteMedicalHistory(int id)
        {
            await _medicalHistoryCommandService.UpdateStatusAsync(id, MedicalHistoryStatusEnum.Completed);
            return Result.SuccessAction("Complete medical history successfully");
        }

        [HttpPut("discharge-bed/{id}")]
        [Authorize(Roles = RolePolicies.MedicalStaffRolePolicy)]
        public async Task<IActionResult> DischargeInpatientBed(int id)
        {
            await _medicalHistoryCommandService.DischargeInpatientBedAsync(id);
            return Result.SuccessAction("Discharge inpatient bed successfully");
        }

        [HttpPut("cancel/{id}")]
        [Authorize(Roles = nameof(RoleEnum.Doctor))]
        public async Task<IActionResult> CancelMedicalHistory(int id)
        {
            await _medicalHistoryCommandService.UpdateStatusAsync(id, MedicalHistoryStatusEnum.Cancelled);
            return Result.SuccessAction("Cancel medical history successfully");
        }
        #endregion
    }
}