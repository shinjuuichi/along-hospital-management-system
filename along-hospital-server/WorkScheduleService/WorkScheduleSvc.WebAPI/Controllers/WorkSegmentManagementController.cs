using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Enums;
using WorkScheduleSvc.BLL.DTOs.WorkSegmentDTOs;
using WorkScheduleSvc.BLL.Interfaces;

namespace WorkScheduleSvc.WebAPI.Controllers
{
    [Authorize(Roles = nameof(RoleEnum.HR))]
    public class WorkSegmentManagementController(IWorkSegmentService workSegmentService) : BaseController
    {
        private readonly IWorkSegmentService _workSegmentService = workSegmentService;

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, UpdateWorkSegmentDTO updateWorkSegmentDTO)
        {
            await _workSegmentService.UpdateAsync(id, updateWorkSegmentDTO);
            return Result.SuccessAction("Work segment updated successfully");
        }
    }
}
