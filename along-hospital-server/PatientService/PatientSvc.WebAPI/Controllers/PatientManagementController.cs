using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatientSvc.BLL.DTOs;
using PatientSvc.BLL.Interfaces;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Commons.Settings;

namespace PatientSvc.WebAPI.Controllers
{
    [Authorize(Roles = RolePolicies.MedicalStaffRolePolicy)]
    public class PatientManagementController(IPatientService patientService) : BaseController
    {
        private readonly IPatientService _patientService = patientService;

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var patients = await _patientService.GetAllAsync();
            return Result.SuccessData(patients);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var patient = await _patientService.GetByIdAsync(id);
            return Result.SuccessData(patient);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePatientAndAccountDTO createPatientAndAccountDTO)
        {
            var createdPatient = await _patientService.CreateWithAccountAsync(createPatientAndAccountDTO);
            return Result.SuccessData(createdPatient, "Create patient successfully");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdatePatientAndAccountDTO updatePatientDTO)
        {
            var updatedPatient = await _patientService.UpdateWithAccountAsync(id, updatePatientDTO);
            return Result.SuccessData(updatedPatient, "Update patient successfully");
        }
    }
}
