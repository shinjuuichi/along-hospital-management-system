using AutoMapper;
using MedicalOrderSvc.BLL.DTOs;
using MedicalOrderSvc.BLL.DTOs.ClinicalMedicalOrderDTOs;
using MedicalOrderSvc.BLL.DTOs.InfusionMedicalOrderDTOs;
using MedicalOrderSvc.BLL.DTOs.InstructionMedicalOrderDTOs;
using MedicalOrderSvc.BLL.DTOs.MedicalOrderDTOs;
using MedicalOrderSvc.BLL.Interfaces;
using MedicalOrderSvc.BLL.StateMachines;
using MedicalOrderSvc.DAL.Enums;
using MedicalOrderSvc.DAL.Models;
using MedicalOrderSvc.DAL.Models.ClinicalMedicalOrders;
using MedicalOrderSvc.DAL.Models.InfusionMedicalOrders;
using MedicalOrderSvc.DAL.Models.InstructionMedicalOrders;
using MessageBroker.Events.BillingEvents;
using MessageBroker.Events.MedicalHistoryEvents;
using SharedLibrary.Base.Data.MongoDb;
using SharedLibrary.Base.MessageBuses;

namespace MedicalOrderSvc.BLL.Implements
{
    public class MedicalOrderService(
        IMongoGenericRepository<MedicalOrder> medicalOrderRepository,
        IMapper mapper,
        IMessageBus messageBus,
        IInfusionMedicalOrderService infusionMedicalOrderService,
        IInstructionMedicalOrderService instructionMedicalOrderService,
        IClinicalMedicalOrderService clinicalMedicalOrderService)
            : IMedicalOrderService
    {
        private readonly IMongoGenericRepository<MedicalOrder> _medicalOrderRepository = medicalOrderRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IMessageBus _messageBus = messageBus;

        private readonly IInfusionMedicalOrderService _infusionMedicalOrderService = infusionMedicalOrderService;
        private readonly IInstructionMedicalOrderService _instructionMedicalOrderService = instructionMedicalOrderService;
        private readonly IClinicalMedicalOrderService _clinicalMedicalOrderService = clinicalMedicalOrderService;

        public async Task<GetMedicalOrderDTO> CreateAsync(CreateMedicalOrderWrapperDTO wrapper)
        {
            if (!Enum.TryParse<MedicalOrderTypeEnum>(wrapper.MedicalOrderType, true, out var type))
            {
                throw new InvalidDataException("Invalid medical order type.");
            }

            await _messageBus.RequestAsync<
                CheckMedicalHistoryExistByIdEvent,
                CheckMedicalHistoryExistByIdContract>(new()
                {
                    Id = wrapper.MedicalHistoryId
                });

            return type switch
            {
                MedicalOrderTypeEnum.Clinical => await this.CreateFromWrapperAsync(
                    wrapper,
                    wrapper.ClinicalMedicalOrder,
                    _clinicalMedicalOrderService.CreateAsync,
                    "Clinical medical order details are required."
                ),

                MedicalOrderTypeEnum.Infusion => await this.CreateFromWrapperAsync(
                    wrapper,
                    wrapper.InfusionMedicalOrder,
                    _infusionMedicalOrderService.CreateAsync,
                    "Infusion medical order details are required."
                ),

                MedicalOrderTypeEnum.Instruction => await this.CreateFromWrapperAsync(
                    wrapper,
                    wrapper.InstructionMedicalOrder,
                    _instructionMedicalOrderService.CreateAsync,
                    "Instruction medical order details are required."
                ),

                _ => throw new InvalidDataException("Unsupported medical order type.")
            };
        }

        public async Task<List<GetMedicalOrderDTO>> GetAllByMedicalHistoryIdAsync(int medicalHistoryId)
        {
            var medicalOrders = await _medicalOrderRepository.GetAllAsync(mo => mo.MedicalHistoryId == medicalHistoryId);
            List<GetMedicalOrderDTO> result = [];

            foreach (var medicalOrder in medicalOrders)
            {
                switch (medicalOrder)
                {
                    case ClinicalMedicalOrder clinicalOrder:
                        var clinicalDto = _mapper.Map<GetClinicalMedicalOrderDTO>(clinicalOrder);
                        result.Add(clinicalDto);
                        break;
                    case InfusionMedicalOrder infusionOrder:
                        var infusionDto = _mapper.Map<GetInfusionMedicalOrderDTO>(infusionOrder);
                        result.Add(infusionDto);
                        break;
                    case InstructionMedicalOrder instructionOrder:
                        var instructionDto = _mapper.Map<GetInstructionMedicalOrderDTO>(instructionOrder);
                        result.Add(instructionDto);
                        break;
                    default:
                        break;
                }
            }

            await _clinicalMedicalOrderService.RequestValueForDTOsAsync(result.OfType<GetClinicalMedicalOrderDTO>().ToList());

            return result;
        }

        public async Task CancelPendingOrDraftByMedicalHistoryIdAsync(int medicalHistoryId)
        {
            var medicalOrders = await _medicalOrderRepository.GetAllAsync(mo => mo.MedicalHistoryId == medicalHistoryId);

            List<MedicalOrder> medicalOrdersToUpdate = [];
            List<string> clinicalMedicalOrderIdsToCancelInvoices = [];

            foreach (var medicalOrder in medicalOrders)
            {
                if (string.IsNullOrWhiteSpace(medicalOrder.Id))
                {
                    continue;
                }

                switch (medicalOrder)
                {
                    case ClinicalMedicalOrder clinicalMedicalOrder when clinicalMedicalOrder.ClinicalMedicalOrderStatus == ClinicalMedicalOrderStatusEnum.Pending:
                        var clinicalStatusStateMachine = new ClinicalMedicalOrderStatusStateMachine(clinicalMedicalOrder);
                        if (clinicalStatusStateMachine.CanFire(ClinicalMedicalOrderStatusEnum.Cancelled))
                        {
                            clinicalStatusStateMachine.Fire(ClinicalMedicalOrderStatusEnum.Cancelled);
                            clinicalMedicalOrderIdsToCancelInvoices.Add(medicalOrder.Id);
                            medicalOrdersToUpdate.Add(medicalOrder);
                        }
                        break;

                    case InstructionMedicalOrder instructionMedicalOrder when instructionMedicalOrder.InstructionMedicalOrderStatus == InstructionMedicalOrderStatusEnum.Draft:
                        var instructionStatusStateMachine = new InstructionMedicalOrderStatusStateMachine(instructionMedicalOrder);
                        if (instructionStatusStateMachine.CanFire(InstructionMedicalOrderStatusEnum.Cancelled))
                        {
                            instructionStatusStateMachine.Fire(InstructionMedicalOrderStatusEnum.Cancelled);
                            medicalOrdersToUpdate.Add(medicalOrder);
                        }
                        break;

                    case InfusionMedicalOrder infusionMedicalOrder:
                        var hasUpdatedInfusionDetail = false;
                        foreach (var detail in infusionMedicalOrder.InfusionMedicalOrderDetails)
                        {
                            if (detail.InfusionMedicalOrderDetailExecutionStatus != InfusionMedicalOrderDetailExecutionStatusEnum.Pending)
                            {
                                continue;
                            }

                            var infusionDetailStateMachine = new InfusionMedicalOrderDetailExecutionStatusStateMachine(detail);
                            if (!infusionDetailStateMachine.CanFire(InfusionMedicalOrderDetailExecutionStatusEnum.Failed))
                            {
                                continue;
                            }

                            infusionDetailStateMachine.Fire(InfusionMedicalOrderDetailExecutionStatusEnum.Failed);
                            hasUpdatedInfusionDetail = true;
                        }

                        if (hasUpdatedInfusionDetail)
                        {
                            medicalOrdersToUpdate.Add(medicalOrder);
                        }
                        break;
                }
            }

            if (medicalOrdersToUpdate.Count == 0)
            {
                return;
            }

            await Task.WhenAll(medicalOrdersToUpdate.Select(medicalOrder => _medicalOrderRepository.UpdateAsync(medicalOrder.Id, medicalOrder)));

            if (clinicalMedicalOrderIdsToCancelInvoices.Count == 0)
            {
                return;
            }

            await _messageBus.PublishAsync(new CancelPendingInvoicesByClinicalMedicalOrderIdsEvent
            {
                ClinicalMedicalOrderIds = clinicalMedicalOrderIdsToCancelInvoices
            });
        }

        private async Task<GetMedicalOrderDTO> CreateFromWrapperAsync<TCreateDTO>(
            CreateMedicalOrderWrapperDTO wrapper,
            TCreateDTO? detail,
            Func<TCreateDTO, Task<GetMedicalOrderDTO>> createFunc,
            string errorMessage)
        {
            if (detail == null)
            {
                throw new InvalidDataException(errorMessage);
            }

            var dto = _mapper.Map(wrapper, detail);
            return await createFunc(dto);
        }
    }
}
