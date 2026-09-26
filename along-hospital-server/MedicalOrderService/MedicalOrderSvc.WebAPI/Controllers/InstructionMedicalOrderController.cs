using MedicalOrderSvc.BLL.DTOs.InstructionMedicalOrderDTOs;
using MedicalOrderSvc.BLL.Interfaces;
using MedicalOrderSvc.DAL.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Enums;

namespace MedicalOrderSvc.WebAPI.Controllers
{
    [Authorize(Roles = nameof(RoleEnum.Doctor))]
    [Route("api/v{version:apiVersion}/medical-order/instruction")]
    public class InstructionMedicalOrderController(
        IInstructionMedicalOrderService instructionMedicalOrderService)
        : BaseController
    {
        private readonly IInstructionMedicalOrderService _instructionMedicalOrderService = instructionMedicalOrderService;

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, UpdateInstructionMedicalOrderDTO updateDTO)
        {
            var result = await _instructionMedicalOrderService.UpdateAsync(id, updateDTO);
            return Result.SuccessData(result, "Instruction medical order updated successfully.");
        }

        [HttpPut("issue/{id}")]
        public async Task<IActionResult> Issue(string id)
        {
            await _instructionMedicalOrderService.UpdateStatusAsync(id, InstructionMedicalOrderStatusEnum.Issued);
            return Result.SuccessAction("Issue instruction medical order successfully.");
        }

        [HttpPut("cancel/{id}")]
        public async Task<IActionResult> Cancel(string id)
        {
            await _instructionMedicalOrderService.UpdateStatusAsync(id, InstructionMedicalOrderStatusEnum.Cancelled);
            return Result.SuccessAction("Cancel instruction medical order successfully.");
        }
    }
}
