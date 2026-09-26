using InpatientResourceSvc.BLL.DTOs.BedOccupancyDTOs;
using InpatientResourceSvc.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;
using SharedLibrary.Commons.Settings;

namespace InpatientResourceSvc.WebAPI.Controllers
{
    [Authorize(Roles = RolePolicies.MedicalStaffRolePolicy)]
    public class BedOccupancyController(IBedOccupancyService _bedOccupancyService) : BaseController
    {
        [HttpGet("rooms")]
        public async Task<IActionResult> GetRoomBoard()
        {
            var rooms = await _bedOccupancyService.GetRoomBoardAsync();
            return Result.SuccessData(rooms);
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignBed(AssignBedDTO assignBedDTO)
        {
            await _bedOccupancyService.AssignBedAsync(assignBedDTO);
            return Result.SuccessAction("Assign bed successfully");
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> TransferBed(TransferBedDTO transferBedDTO)
        {
            await _bedOccupancyService.TransferBedAsync(transferBedDTO);
            return Result.SuccessAction("Transfer bed successfully");
        }
    }
}