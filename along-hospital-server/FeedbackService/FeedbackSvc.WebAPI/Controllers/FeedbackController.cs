using FeedbackSvc.BLL.DTOs.FeedbackDTOs;
using FeedbackSvc.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Enums;

namespace FeedbackSvc.WebAPI.Controllers
{
    public class FeedbackController(
        IFeedbackService feedbackService)
        : CrudController<CreateFeedbackDTO, UpdateFeedbackDTO, GetFeedbackDTO>(feedbackService)
    {
        protected override string? EntityName => "Feedback";
        private readonly IFeedbackService _feedbackService = feedbackService;

        [Authorize(Roles = nameof(RoleEnum.Patient))]
        public override Task<IActionResult> Create(CreateFeedbackDTO createDTO)
        {
            return base.Create(createDTO);
        }

        [Authorize(Roles = nameof(RoleEnum.Patient))]
        public override Task<IActionResult> Update(int id, UpdateFeedbackDTO updateDTO)
        {
            return base.Update(id, updateDTO);
        }

        [HttpGet("medicine/{medicineId}")]
        public async Task<IActionResult> GetFeedbackByMedicineId(int medicineId)
        {
            var result = await _feedbackService.GetFeedbackByMedicineIdAsync(medicineId);
            return Result.SuccessData(result);
        }
    }
}