using FeedbackSvc.BLL.DTOs.FeedbackReportDTOs;
using FeedbackSvc.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Enums;

namespace FeedbackSvc.WebAPI.Controllers
{
    [Authorize(Roles = nameof(RoleEnum.Patient))]
    public class FeedbackReportController(IFeedbackReportService feedbackReportService) : BaseController
    {
        private readonly IFeedbackReportService _feedbackReportService = feedbackReportService;

        [HttpPost]
        public async Task<IActionResult> Create(CreateFeedbackReportDTO createDTO)
        {
            await _feedbackReportService.CreateAsync(createDTO);
            return Result.SuccessAction("Report feedback successfully");
        }
    }
}