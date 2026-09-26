using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayrollSvc.BLL.DTOs.PayrollDTOs;
using PayrollSvc.BLL.FilterDTOs;
using PayrollSvc.BLL.Interfaces;
using PayrollSvc.DAL.Enums;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Commons.Settings;
using SharedLibrary.Enums;

namespace PayrollSvc.WebAPI.Controllers
{
    public class PayrollManagementController(
        IPayrollService payrollService)
    : GetController<GetPayrollDTO, PayrollFilterDTO>(payrollService)
    {
        private readonly IPayrollService _payrollService = payrollService;

        //Dont care about this
        [HttpPost]
        public async Task<IActionResult> Create(CreatePayrollDTO createPayrollDTO)
        {
            var result = await _payrollService.CreateAsync(createPayrollDTO);
            return Result.SuccessData(result, "Payroll created successfully");
        }

        [Authorize(Roles = nameof(RoleEnum.Accountant))]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdatePayrollDTO updatePayrollDTO)
        {
            var result = await _payrollService.UpdateAsync(id, updatePayrollDTO);
            return Result.SuccessData(result, "Payroll updated successfully");
        }

        [Authorize(Roles = nameof(RoleEnum.Accountant))]
        [HttpPut("pending")]
        public async Task<IActionResult> Pending([FromQuery] List<int> ids)
        {
            await _payrollService.UpdatePayrollStatusAsync(ids, PayrollStatusEnum.Pending);
            return Result.SuccessAction("Payrolls updated to pending successfully.");
        }

        [Authorize(Roles = nameof(RoleEnum.Manager))]
        [HttpPut("approve")]
        public async Task<IActionResult> Approve([FromQuery] List<int> ids)
        {
            await _payrollService.UpdatePayrollStatusAsync(ids, PayrollStatusEnum.Approved);
            return Result.SuccessAction("Payrolls updated to approved successfully.");
        }

        [Authorize(Roles = nameof(RoleEnum.Accountant))]
        public override async Task<IActionResult> GetAll()
        {
            return await base.GetAll();
        }

        [Authorize(Roles = nameof(RoleEnum.Accountant))]
        public override async Task<IActionResult> GetById(int id)
        {
            return await base.GetById(id);
        }

        [Authorize(Roles = RolePolicies.PayrollManagementRolePolicy)]
        public override async Task<IActionResult> GetAllPaginated(PayrollFilterDTO filterDTO)
        {
            return await base.GetAllPaginated(filterDTO);
        }
    }
}
