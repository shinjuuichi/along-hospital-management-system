using MedicalOrderSvc.BLL.DTOs;
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
    public class MedicalOrderController(IMedicalOrderService medicalOrderService)
        : BaseController
    {
        private readonly IMedicalOrderService _medicalOrderService = medicalOrderService;

        [HttpPost]
        public async Task<IActionResult> Create(CreateMedicalOrderWrapperDTO createWrapperDTO)
        {
            var medicalOrderDTO = await _medicalOrderService.CreateAsync(createWrapperDTO);
            return Result.SuccessData(medicalOrderDTO, "Medical order created successfully.");
        }

        [HttpPost("instruction/draft")]
        public async Task<IActionResult> CreateInstructionMedicalOrderAsDraft(CreateMedicalOrderWrapperDTO createWrapperDTO)
        {
            if (createWrapperDTO.InstructionMedicalOrder == null)
            {
                throw new InvalidDataException("InstructionMedicalOrder data must be provided for creating an instruction medical order as draft.");
            }

            createWrapperDTO.MedicalOrderType = nameof(MedicalOrderTypeEnum.Instruction);
            createWrapperDTO.InstructionMedicalOrder.InstructionMedicalOrderStatus = nameof(InstructionMedicalOrderStatusEnum.Draft);

            var medicalOrderDTO = await _medicalOrderService.CreateAsync(createWrapperDTO);
            return Result.SuccessData(medicalOrderDTO, "Instruction medical order created as draft successfully.");
        }

        [HttpGet("{medicalHistoryId}")]
        public async Task<IActionResult> GetAllByMedicalHistoryId(int medicalHistoryId)
        {
            var medicalOrders = await _medicalOrderService.GetAllByMedicalHistoryIdAsync(medicalHistoryId);
            return Result.SuccessData(medicalOrders);
        }
    }
}