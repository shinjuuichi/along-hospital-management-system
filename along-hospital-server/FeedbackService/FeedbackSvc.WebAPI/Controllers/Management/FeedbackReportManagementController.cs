using FeedbackSvc.BLL.DTOs.FeedbackReportDTOs;
using FeedbackSvc.BLL.FilterDTOs;
using FeedbackSvc.BLL.Interfaces;
using FeedbackSvc.DAL.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Enums;

namespace FeedbackSvc.WebAPI.Controllers.Management
{
    [Authorize(Roles = nameof(RoleEnum.HotlineAgent))]
    public class FeedbackReportManagementController(
        IFeedbackReportService feedbackReportService)
        : GetController<GetFeedbackReportDTO, FeedbackReportFilterDTO>(feedbackReportService)
    {
        private readonly IFeedbackReportService _feedbackReportService = feedbackReportService;

        [HttpPut("resolve/{id}")]
        public async Task<IActionResult> ResolveFeedbackReport(int id)
        {
            await _feedbackReportService.UpdateStatusAsync(id, FeedbackReportStatusEnum.Resolved);
            return Result.SuccessAction("Update FeedbackReport to resolved successfully");
        }

        [HttpPut("reject/{id}")]
        public async Task<IActionResult> RejectFeedbackReport(int id)
        {
            await _feedbackReportService.UpdateStatusAsync(id, FeedbackReportStatusEnum.Rejected);
            return Result.SuccessAction("Update FeedbackReport to rejected successfully");
        }
    }
}