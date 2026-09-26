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
    [Route("api/v{version:apiVersion}/medical-order/infusion")]
    public class InfusionMedicalOrderController(
        IInfusionMedicalOrderService infusionMedicalOrderService)
            : BaseController
    {
        private readonly IInfusionMedicalOrderService _infusionMedicalOrderService = infusionMedicalOrderService;

        [HttpPut("{medicalOrderId}/detail/complete/{medicineId}")]
        public async Task<IActionResult> CompleteInfusionMedicalOrderDetail(
            string medicalOrderId,
            int medicineId)
        {
            await _infusionMedicalOrderService.UpdateDetailStatusAsync(
                medicalOrderId,
                medicineId,
                InfusionMedicalOrderDetailExecutionStatusEnum.Completed);
            return Result.SuccessAction("Infusion medical order detail marked as completed successfully.");
        }

        [HttpPut("{medicalOrderId}/detail/fail/{medicineId}")]
        public async Task<IActionResult> FailInfusionMedicalOrderDetail(
            string medicalOrderId,
            int medicineId)
        {
            await _infusionMedicalOrderService.UpdateDetailStatusAsync(
                medicalOrderId,
                medicineId,
                InfusionMedicalOrderDetailExecutionStatusEnum.Failed);
            return Result.SuccessAction("Infusion medical order detail marked as failed successfully.");
        }
    }
}
