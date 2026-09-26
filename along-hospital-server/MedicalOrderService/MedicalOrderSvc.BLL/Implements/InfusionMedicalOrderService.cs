using AutoMapper;
using MedicalOrderSvc.BLL.DTOs.InfusionMedicalOrderDTOs;
using MedicalOrderSvc.BLL.DTOs.MedicalOrderDTOs;
using MedicalOrderSvc.BLL.Interfaces;
using MedicalOrderSvc.BLL.StateMachines;
using MedicalOrderSvc.DAL.Enums;
using MedicalOrderSvc.DAL.Models.InfusionMedicalOrders;
using MedicalOrderSvc.DAL.Models.Snapshots;
using MessageBroker.Contracts.MedicineContracts;
using MessageBroker.Events.MedicineEvents;
using SharedLibrary.Base.Data.MongoDb;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Commons.Exceptions;

namespace MedicalOrderSvc.BLL.Implements
{
    public class InfusionMedicalOrderService(
        IMongoGenericRepository<InfusionMedicalOrder> infusionMedicalOrderRepository,
        IMapper mapper,
        IMessageBus messageBus)
            : IInfusionMedicalOrderService
    {
        private readonly IMongoGenericRepository<InfusionMedicalOrder> _infusionMedicalOrderRepository = infusionMedicalOrderRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IMessageBus _messageBus = messageBus;

        public async Task<GetMedicalOrderDTO> CreateAsync(CreateInfusionMedicalOrderDTO createDTO)
        {
            var medicineIds = createDTO.InfusionMedicalOrderDetails
                .Select(detail => detail.MedicineId)
                .ToList();

            if (medicineIds.Count == 0)
            {
                throw new InvalidDataException("At least one medicine must be included in infusion medical order details.");
            }

            var hasDuplicatedMedicine = medicineIds.Count != medicineIds.Distinct().Count();
            if (hasDuplicatedMedicine)
            {
                throw new InvalidDataException("Duplicate medicine IDs are not allowed in infusion medical order details.");
            }

            var medicineContracts = await _messageBus.RequestAsync<
                GetListMedicineDataByIdsEvent,
                GetListMedicineDataByIdsContract>(new()
                {
                    Ids = medicineIds
                });

            var medicineDict = medicineContracts.Data.ToDictionary(m => m.Id);

            var infusionMedicalOrder = _mapper.Map<InfusionMedicalOrder>(createDTO);
            foreach (var detail in infusionMedicalOrder.InfusionMedicalOrderDetails)
            {
                if (!medicineDict.TryGetValue(detail.MedicineId, out var medicine))
                {
                    throw new DataNotFoundException("Medicine", detail.MedicineId);
                }

                detail.MedicineSnapshot = _mapper.Map<MedicineSnapshot>(medicine);
            }

            var createdInfusionMedicalOrder = await _infusionMedicalOrderRepository.AddAsync(infusionMedicalOrder);

            return _mapper.Map<GetInfusionMedicalOrderDTO>(createdInfusionMedicalOrder);
        }

        public async Task UpdateDetailStatusAsync(string medicalOrderId, int medicineId, InfusionMedicalOrderDetailExecutionStatusEnum infusionMedicalOrderDetailExecutionStatus)
        {
            var infusionMedicalOrder = await _infusionMedicalOrderRepository.GetByIdAsync(medicalOrderId)
                ?? throw new DataNotFoundException(typeof(InfusionMedicalOrder), medicalOrderId);

            var infusionMedicalOrderDetail = infusionMedicalOrder.InfusionMedicalOrderDetails
                .FirstOrDefault(detail => detail.MedicineId == medicineId)
                    ?? throw new DataNotFoundException($"Medicine ({medicineId}) was not found in Infusion Medical Order ({medicalOrderId})!");

            this.UpdateDetailStatus(infusionMedicalOrderDetail, infusionMedicalOrderDetailExecutionStatus);
            await _infusionMedicalOrderRepository.UpdateAsync(medicalOrderId, infusionMedicalOrder);
        }

        private void UpdateDetailStatus(
            InfusionMedicalOrderDetail infusionMedicalOrderDetail,
            InfusionMedicalOrderDetailExecutionStatusEnum infusionMedicalOrderDetailExecutionStatus)
        {
            var stateMachine = new InfusionMedicalOrderDetailExecutionStatusStateMachine(infusionMedicalOrderDetail);
            if (!stateMachine.CanFire(infusionMedicalOrderDetailExecutionStatus))
            {
                throw new InvalidDataException(
                    $"Cannot change infusion medical order detail execution status from {infusionMedicalOrderDetail.InfusionMedicalOrderDetailExecutionStatus} to {infusionMedicalOrderDetailExecutionStatus}.");
            }

            try
            {
                stateMachine.Fire(infusionMedicalOrderDetailExecutionStatus);
            }
            catch (Exception e)
            {
                throw new InvalidDataException($"Failed to change infusion medical order detail execution status: {e.Message}");
            }
        }
    }
}
