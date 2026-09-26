using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QueueSvc.BLL.DTOs;
using QueueSvc.BLL.Interfaces;
using QueueSvc.DAL.Enums;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Commons.Settings;
using SharedLibrary.Enums;

namespace QueueSvc.WebAPI.Controllers
{
    [Authorize(Roles = RolePolicies.QueueManagementRolePolicy)]
    public class QueueManagementController(
        IQueueCommandService queueCommandService) : BaseController
    {
        private readonly IQueueCommandService _queueCommandService = queueCommandService;

        [Authorize(Roles = nameof(RoleEnum.Receptionist))]
        [HttpPut("assign-medical-history/{id}")]
        public async Task<IActionResult> AssignMedicalHistory(int id, AssignMedicalHistoryToQueueDTO assignDTO)
        {
            await _queueCommandService.AssignMedicalHistoryAsync(id, assignDTO);
            return Result.SuccessAction("Medical history assigned to queue successfully.");
        }

        [HttpPut("call/{id}")]
        public async Task<IActionResult> Call(int id)
        {
            await _queueCommandService.UpdateStatusAsync(id, QueueStatusEnum.Called);
            return Result.SuccessAction("Queue called successfully.");
        }

        [HttpPut("uncall/{id}")]
        public async Task<IActionResult> Uncall(int id)
        {
            await _queueCommandService.UpdateStatusAsync(id, QueueStatusEnum.Waiting);
            return Result.SuccessAction("Queue moved back to waiting successfully.");
        }

        [HttpPut("start/{id}")]
        public async Task<IActionResult> Start(int id)
        {
            await _queueCommandService.UpdateStatusAsync(id, QueueStatusEnum.InProgress);
            return Result.SuccessAction("Queue started successfully.");
        }

        [HttpPut("await-results/{id}")]
        public async Task<IActionResult> AwaitResults(int id)
        {
            await _queueCommandService.UpdateStatusAsync(id, QueueStatusEnum.AwaitingResults);
            return Result.SuccessAction("Queue set to awaiting results successfully.");
        }

        [HttpPut("complete/{id}")]
        public async Task<IActionResult> Complete(int id)
        {
            await _queueCommandService.UpdateStatusAsync(id, QueueStatusEnum.Completed);
            return Result.SuccessAction("Queue completed successfully.");
        }

        [HttpPut("cancel/{id}")]
        public async Task<IActionResult> Cancel(int id)
        {
            await _queueCommandService.UpdateStatusAsync(id, QueueStatusEnum.Cancelled);
            return Result.SuccessAction("Queue cancelled successfully.");
        }
    }
}
