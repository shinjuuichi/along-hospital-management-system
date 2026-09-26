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
    [Route("api/v{version:apiVersion}/medical-order/clinical")]
    public class ClinicalMedicalOrderController(
        IClinicalMedicalOrderService clinicalMedicalOrderService)
            : BaseController
    {
        private readonly IClinicalMedicalOrderService _clinicalMedicalOrderService = clinicalMedicalOrderService;

        [HttpPut("cancel/{id}")]
        public async Task<IActionResult> CancelClinicalMedicalOrder(string id)
        {
            await _clinicalMedicalOrderService.UpdateStatusAsync(id, ClinicalMedicalOrderStatusEnum.Cancelled);
            return Result.SuccessAction("Cancel clinical medical order successfully.");
        }

        [HttpPut("{medicalOrderId}/detail/complete/{medicalServiceId}")]
        public async Task<IActionResult> CompleteClinicalMedicalOrderDetail(
            string medicalOrderId,
            int medicalServiceId)
        {
            await _clinicalMedicalOrderService.UpdateDetailStatusAsync(
                medicalOrderId,
                medicalServiceId,
                ClinicalMedicalOrderDetailStatusEnum.Completed);
            return Result.SuccessAction("Clinical medical order detail marked as completed successfully.");
        }

        [HttpPut("{medicalOrderId}/detail/fail/{medicalServiceId}")]
        public async Task<IActionResult> FailClinicalMedicalOrderDetail(
            string medicalOrderId,
            int medicalServiceId,
            string? reason = null)
        {
            await _clinicalMedicalOrderService.UpdateDetailStatusAsync(
                medicalOrderId,
                medicalServiceId,
                ClinicalMedicalOrderDetailStatusEnum.Failed,
                reason);
            return Result.SuccessAction("Clinical medical order detail marked as failed successfully.");
        }
    }
}
