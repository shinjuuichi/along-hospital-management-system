using Microsoft.AspNetCore.Mvc;
using QueueSvc.BLL.DTOs.CreateQueueDTOs;
using QueueSvc.BLL.Interfaces;
using SharedLibrary.Base.Controllers;
using SharedLibrary.Commons.Results;

namespace QueueSvc.WebAPI.Controllers
{
    public class QueueController(
        IQueueQueryService queueQueryService,
        IQueueCommandService queueCommandService) : BaseController
    {
        private readonly IQueueQueryService _queueQueryService = queueQueryService;
        private readonly IQueueCommandService _queueCommandService = queueCommandService;

        [HttpGet("all")]
        public async Task<IActionResult> GetAllTodayWorkingQueue()
        {
            var result = await _queueQueryService.GetAllTodayWorkingQueueAsync();
            return Result.SuccessData(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            var queueDTO = await _queueCommandService.CreateAsync();
            return Result.SuccessData(queueDTO, "Queue created successfully.");
        }

        [HttpPost("qr")]
        public async Task<IActionResult> CreateFromQRData(CreateQueueFromQRDataDTO createDTO)
        {
            var queueDTO = await _queueCommandService.CreateFromQRDataAsync(createDTO);
            return Result.SuccessData(queueDTO, "Queue created from QR data successfully.");
        }
    }
}