using AutoMapper;
using MedicalOrderSvc.BLL.DTOs.InstructionMedicalOrderDTOs;
using MedicalOrderSvc.BLL.DTOs.MedicalOrderDTOs;
using MedicalOrderSvc.BLL.Interfaces;
using MedicalOrderSvc.BLL.StateMachines;
using MedicalOrderSvc.DAL.Enums;
using MedicalOrderSvc.DAL.Models.InstructionMedicalOrders;
using SharedLibrary.Base.Data.MongoDb;
using SharedLibrary.Commons.Exceptions;

namespace MedicalOrderSvc.BLL.Implements
{
    public class InstructionMedicalOrderService(
        IMongoGenericRepository<InstructionMedicalOrder> instructionMedicalOrderRepository,
        IMapper mapper)
            : IInstructionMedicalOrderService
    {
        private readonly IMongoGenericRepository<InstructionMedicalOrder> _instructionMedicalOrderRepository = instructionMedicalOrderRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<GetMedicalOrderDTO> CreateAsync(CreateInstructionMedicalOrderDTO createDTO)
        {
            if (string.IsNullOrEmpty(createDTO.InstructionMedicalOrderStatus))
            {
                createDTO.InstructionMedicalOrderStatus = nameof(InstructionMedicalOrderStatusEnum.Issued);
            }

            if (!Enum.TryParse<InstructionMedicalOrderStatusEnum>(createDTO.InstructionMedicalOrderStatus, out var instructionMedicalOrderStatus)
                || instructionMedicalOrderStatus is InstructionMedicalOrderStatusEnum.Cancelled)
            {
                throw new InvalidDataException($"Invalid instruction medical order status: {createDTO.InstructionMedicalOrderStatus}. " +
                    $"Allowed values are: {nameof(InstructionMedicalOrderStatusEnum.Draft)}, {nameof(InstructionMedicalOrderStatusEnum.Issued)}.");
            }

            var isAllNull = createDTO.PositionOrder == null
                && createDTO.RespiratorySupportOrder == null
                && createDTO.NutritionOrder == null
                && createDTO.NursingCareOrder == null;

            if (isAllNull)
            {
                throw new InvalidDataException("At least one order type must be provided.");
            }

            var instructionMedicalOrder = _mapper.Map<InstructionMedicalOrder>(createDTO);
            var createdInstructionMedicalOrder = await _instructionMedicalOrderRepository.AddAsync(instructionMedicalOrder);

            return _mapper.Map<GetInstructionMedicalOrderDTO>(createdInstructionMedicalOrder);
        }

        public async Task<GetMedicalOrderDTO> UpdateAsync(string medicalOrderId, UpdateInstructionMedicalOrderDTO updateDTO)
        {
            var instructionMedicalOrder = await _instructionMedicalOrderRepository.GetByIdAsync(medicalOrderId)
                ?? throw new DataNotFoundException(typeof(InstructionMedicalOrder), medicalOrderId);
            if (instructionMedicalOrder.InstructionMedicalOrderStatus is not InstructionMedicalOrderStatusEnum.Draft)
            {
                throw new InvalidDataException("Only instruction medical orders in Draft status can be updated.");
            }

            _mapper.Map(updateDTO, instructionMedicalOrder);
            await _instructionMedicalOrderRepository.UpdateAsync(medicalOrderId, instructionMedicalOrder);
            return _mapper.Map<GetInstructionMedicalOrderDTO>(instructionMedicalOrder);
        }

        #region State Machine Management
        public async Task UpdateStatusAsync(string medicalOrderId, InstructionMedicalOrderStatusEnum instructionMedicalOrderStatus)
        {
            var instructionMedicalOrder = await _instructionMedicalOrderRepository.GetByIdAsync(medicalOrderId)
                ?? throw new DataNotFoundException(typeof(InstructionMedicalOrder), medicalOrderId);

            this.UpdateStatus(instructionMedicalOrder, instructionMedicalOrderStatus);
            await _instructionMedicalOrderRepository.UpdateAsync(medicalOrderId, instructionMedicalOrder);
        }

        private void UpdateStatus(InstructionMedicalOrder instructionMedicalOrder, InstructionMedicalOrderStatusEnum instructionMedicalOrderStatus)
        {
            var stateMachine = new InstructionMedicalOrderStatusStateMachine(instructionMedicalOrder);
            if (!stateMachine.CanFire(instructionMedicalOrderStatus))
            {
                throw new InvalidDataException(
                    $"Cannot change instruction medical order status from {instructionMedicalOrder.InstructionMedicalOrderStatus} to {instructionMedicalOrderStatus}.");
            }

            try
            {
                stateMachine.Fire(instructionMedicalOrderStatus);
            }
            catch (Exception e)
            {
                throw new InvalidDataException($"Failed to change instruction medical order status: {e.Message}");
            }
        }
        #endregion
    }
}
