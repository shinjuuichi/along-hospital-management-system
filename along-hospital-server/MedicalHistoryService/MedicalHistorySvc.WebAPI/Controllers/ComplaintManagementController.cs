using MedicalHistorySvc.BLL.FilterDTOs;
using MedicalHistorySvc.BLL.Interfaces;
using MedicalHistorySvc.DAL.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Commons.Settings;

namespace MedicalHistorySvc.WebAPI.Controllers
{
    [Authorize(Roles = RolePolicies.CustomerSupportRolePolicy)]
    public class ComplaintManagementController(IComplaintService complaintService) : BaseController
    {
        private readonly IComplaintService _complaintService = complaintService;

        [HttpGet]
        public async Task<IActionResult> GetAll(ComplaintFilterDTO complaintFilterDTO)
        {
            var complaints = await _complaintService.GetAllAsync(complaintFilterDTO);
            return Result.SuccessData(complaints);
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetComplaintSummary(string yearWeek)
        {
            var summary = await _complaintService.GetComplaintSummaryByWeekAsync(yearWeek);
            return Result.SuccessData(summary);
        }

        [HttpPut("draft/{id}")]
        public async Task<IActionResult> DraftComplaint(int id, string? response = null)
        {
            await _complaintService.UpdateStatusAsync(id, ComplaintResolveStatusEnum.Draft, response);
            return Result.SuccessAction("Mark complaint as drafted successfully");
        }

        [HttpPut("resolve/{id}")]
        public async Task<IActionResult> ResolveComplaint(int id, string? response = null)
        {
            await _complaintService.UpdateStatusAsync(id, ComplaintResolveStatusEnum.Resolved, response);
            return Result.SuccessAction("Resolve complaint successfully");
        }

        [HttpPut("close/{id}")]
        public async Task<IActionResult> CloseComplaint(int id)
        {
            await _complaintService.UpdateStatusAsync(id, ComplaintResolveStatusEnum.Closed);
            return Result.SuccessAction("Close complaint successfully");
        }

        [HttpPut("classify/{id}")]
        public async Task<IActionResult> ClassifyComplaint(int id, string type)
        {
            if (!Enum.TryParse<ComplaintTypeEnum>(type, true, out var complaintType))
            {
                throw new InvalidDataException("Invalid complaint type");
            }

            await _complaintService.ClassifyAsync(id, complaintType);
            return Result.SuccessAction("Classify complaint successfully");
        }
    }
}
