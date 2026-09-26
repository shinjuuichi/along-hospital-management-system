using AutoMapper;
using MessageBroker.Contracts.AuthAccountContracts.GetUserDataContracts;
using MessageBroker.Contracts.InventoryContracts;
using MessageBroker.Contracts.MedicineContracts;
using MessageBroker.Contracts.PaymentContracts;
using MessageBroker.Contracts.VoucherContracts;
using MessageBroker.Events.AuthAccountEvents.GetUserDataEvents;
using MessageBroker.Events.InventoryEvents;
using MessageBroker.Events.MedicineEvents;
using MessageBroker.Events.OrderEvents;
using MessageBroker.Events.PaymentEvents;
using MessageBroker.Events.VoucherEvents;
using OrderSvc.BLL.DTOs;
using OrderSvc.BLL.Interfaces;
using OrderSvc.BLL.StateMachines;
using OrderSvc.DAL.Enums;
using OrderSvc.DAL.Models;
using OrderSvc.DAL.Models.Snapshots;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Base.Services;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Commons.Filters;
using SharedLibrary.Commons.Results;
using SharedLibrary.Services.Interfaces;
using Order = OrderSvc.DAL.Models.Order;

namespace OrderSvc.BLL.Implements
{
    public class OrderService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMessageBus messageBus,
        ICurrentUserService currentUserService)
        : BaseService<Order, CreateOrderDTO, NotUpdateOrderDTO, GetOrderDTO>(
            unitOfWork,
            mapper,
            includes: [nameof(Order.OrderDetails)]),
                IOrderService
    {
        private const string DEFAULT_PAYMENT_STATUS_SUCCESS = "Success";
        private const int OVERDUE_DAYS = 7;

        private readonly IMessageBus _messageBus = messageBus;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        #region Get Orders
        public async Task<PaginationResult<GetOrderDTO>> GetAllPaginatedByUserIdAsync(OrderFilterDTO orderFilterDTO)
        {
            var userId = _currentUserService.UserId;

            var (total, orders) = await _repository.GetAllPaginatedAsync(
                x => x.PatientId == userId,
                orderFilterDTO.Filter,
                orderFilterDTO.Sort,
                orderFilterDTO.Page,
                orderFilterDTO.PageSize,
                _includes);

            var orderDTOs = _mapper.Map<List<GetOrderDTO>>(orders);
            return new PaginationResult<GetOrderDTO>(total, orderFilterDTO.PageSize, orderDTOs);
        }

        public async Task<GetOrderDTO?> GetOrderByCurrentUserAsync(int orderId)
        {
            var userId = _currentUserService.UserId;

            var order = await _repository
                .GetByConditionAsync(x => x.Id == orderId && x.PatientId == userId, _includes);

            if (order == null)
            {
                return null;
            }

            return _mapper.Map<GetOrderDTO>(order);
        }

        public override async Task<PaginationResult<GetOrderDTO>> GetAllPaginatedAsync(FilterDTO filterDTO)
        {
            var result = await base.GetAllPaginatedAsync(filterDTO);

            if (result.Collection.Count == 0)
            {
                return result;
            }

            var patientIds = result.Collection.Select(o => o.PatientId).Distinct().ToList();

            var getListUserDataContract = await _messageBus
                .RequestAsync<GetListUserDataByUserIdsEvent, GetListUserDataByUserIdsContract>(
                    new() { UserIds = patientIds });

            var userDict = getListUserDataContract.Data.ToDictionary(u => u.UserId, u => u);

            foreach (var dto in result.Collection)
            {
                if (userDict.TryGetValue(dto.PatientId, out var user))
                {
                    dto.PatientName = user.Name;
                    dto.PatientPhone = user.Phone;
                    dto.PatientEmail = user.Email;
                    dto.PatientImage = user.Image;
                    dto.PatientAddress = user.Address;
                }
            }

            return result;
        }
        #endregion

        #region Create Orders
        public override async Task<GetOrderDTO> CreateAsync(CreateOrderDTO createOrderDTO)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var skuCodes = createOrderDTO.Details
                    .Select(d => d.SKUCode)
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .OfType<string>()
                    .Distinct()
                    .ToList();

                if (skuCodes.Count == 0)
                {
                    throw new ValidationFailureException("Cart items are empty.");
                }

                var checkQuantityEvent = new CheckQuantityOfListSKUDataEvent
                {
                    SKUEventItems = _mapper.Map<List<CheckQuantityOfSKUEventItem>>(
                        createOrderDTO.Details.Where(d => !string.IsNullOrWhiteSpace(d.SKUCode)))
                };

                await _messageBus.RequestAsync<
                    CheckQuantityOfListSKUDataEvent,
                    CheckQuantityOfListSKUDataContract>(checkQuantityEvent);

                var medicineSkuData = await _messageBus.RequestAsync<
                    GetListMedicineSKUDataBySKUCodesEvent,
                    GetListMedicineSKUDataContract>(new() { SKUCodes = skuCodes });

                var skuDict = medicineSkuData.Data
                        .Where(x => !string.IsNullOrWhiteSpace(x.SKUCode))
                        .ToDictionary(x => x.SKUCode is string code ? code
                            : throw new DataNotFoundException("SKUCode must not be null or empty."),
                            x => x, StringComparer.OrdinalIgnoreCase);

                foreach (var detail in createOrderDTO.Details)
                {
                    if (string.IsNullOrWhiteSpace(detail.SKUCode))
                    {
                        continue;
                    }

                    if (!skuDict.ContainsKey(detail.SKUCode))
                    {
                        throw new DataNotFoundException($"Medicine SKU not found with SKUCode = {detail.SKUCode}");
                    }
                }

                var medicinesForVoucher = createOrderDTO.Details
                    .Where(d => !string.IsNullOrWhiteSpace(d.SKUCode))
                    .Select(d =>
                    {
                        if (string.IsNullOrWhiteSpace(d.SKUCode))
                        {
                            throw new DataNotFoundException($"Medicine SKU not found with SKUCode = {d.SKUCode}");
                        }

                        var sku = skuDict[d.SKUCode];

                        return new MedicineDetailEventItem
                        {
                            MedicineId = sku.MedicineId,
                            SKUCode = sku.SKUCode,
                            Price = sku.Price,
                            Quantity = d.Quantity
                        };
                    })
                    .ToList();

                var applyVoucherContract = await _messageBus.RequestAsync<ApplyVoucherEvent, ApplyVoucherContract>(new()
                {
                    PatientId = createOrderDTO.PatientId,
                    VoucherCode = createOrderDTO.VoucherCode,
                    Medicines = medicinesForVoucher
                });

                var order = new Order
                {
                    PatientId = createOrderDTO.PatientId,
                    VoucherCode = createOrderDTO.VoucherCode,
                    IsPickupAtStore = createOrderDTO.IsPickupAtStore,
                    OrderDetails = [],
                    OriginPrice = applyVoucherContract.OriginalTotalPrice,
                    TotalDiscountAmount = applyVoucherContract.TotalSaved,
                    FinalPrice = applyVoucherContract.FinalTotalPrice
                };

                if (applyVoucherContract.FinalTotalPrice == 0)
                {
                    this.UpdateOrderStatus(order, OrderStatusEnum.Paid);
                }

                var medicineDiscountDict = applyVoucherContract.MedicineDiscounts
                    .Where(d => d.SKUCode is string skuCode && !string.IsNullOrWhiteSpace(skuCode))
                    .GroupBy(d => d.SKUCode is string skuCode ? skuCode
                        : throw new DataNotFoundException("SKUCode must not be null or empty."), StringComparer.OrdinalIgnoreCase)
                    .ToDictionary(g => g.Key, g => g.First(), StringComparer.OrdinalIgnoreCase);

                var paymentItems = new List<PaymentEventItem>();

                foreach (var detail in createOrderDTO.Details)
                {
                    if (string.IsNullOrWhiteSpace(detail.SKUCode))
                    {
                        continue;
                    }

                    var sku = skuDict[detail.SKUCode];

                    if (string.IsNullOrWhiteSpace(sku.SKUCode))
                    {
                        continue;
                    }

                    medicineDiscountDict.TryGetValue(detail.SKUCode, out var discountDetail);

                    var unitPrice = sku.Price;
                    double discountAmountPerUnit = 0;
                    double finalUnitPrice = unitPrice;

                    if (discountDetail is not null)
                    {
                        var qty = Math.Max(1, detail.Quantity);
                        discountAmountPerUnit = discountDetail.MedicineDiscountAmount / qty;
                        finalUnitPrice = (discountDetail.OriginalPrice - discountDetail.MedicineDiscountAmount) / qty;
                    }

                    paymentItems.Add(new PaymentEventItem
                    {
                        ServiceName = sku.MedicineName,
                        UnitPrice = finalUnitPrice,
                        Quantity = detail.Quantity
                    });

                    order.OrderDetails.Add(new OrderDetail
                    {
                        OrderId = order.Id,
                        SKUCode = sku.SKUCode,
                        Quantity = detail.Quantity,
                        UnitPrice = unitPrice,
                        DiscountAmount = discountAmountPerUnit,
                        MedicineSnapshot = new MedicineSnapshot
                        {
                            MedicineName = sku.MedicineName,
                            MedicineBrand = sku.MedicineBrand,
                            MedicineImages = sku.MedicineImages ?? [],
                            MedicineUnit = sku.MedicineUnit
                        }
                    });
                }

                paymentItems = this.DistributeDiscountToPaymentItems(paymentItems, applyVoucherContract.FinalTotalPrice);

                var paymentEvent = new CreatePaymentEvent
                {
                    Description = createOrderDTO.Description,
                    Provider = createOrderDTO.PaymentType,
                    PaymentEventItems = paymentItems
                };

                var createPaymentContract = await _messageBus
                    .RequestAsync<CreatePaymentEvent, CreatePaymentContract>(paymentEvent);

                order.TransactionId = createPaymentContract.TransactionId;

                await _repository.AddAsync(order);
                await _unitOfWork.SaveChangeAsync();

                var decreaseInventoryEvent = new DecreaseInventoryQuantityEvent
                {
                    DecreaseInventoryQuantityEventItems = _mapper.Map<List<DecreaseInventoryQuantityEventItem>>(
                        createOrderDTO.Details.Where(d => !string.IsNullOrWhiteSpace(d.SKUCode)))
                };

                await _messageBus.RequestAsync<
                    DecreaseInventoryQuantityEvent,
                    DecreaseMedicineQuantityContract>(decreaseInventoryEvent);

                await _unitOfWork.CommitTransactionAsync();

                var createdOrder = _mapper.Map<GetOrderDTO>(order);
                createdOrder.PaymentUrl = createPaymentContract.PaymentUrl;
                return createdOrder;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
        #endregion

        #region Order Actions
        public async Task<string> RepayOrderAsync(int orderId, string paymentType)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var order = await _repository.GetByIdAsync(orderId, _includes)
                    ?? throw new DataNotFoundException(typeof(Order), orderId);

                if (order.OrderStatus != OrderStatusEnum.Cancelled && order.OrderStatus != OrderStatusEnum.Unpaid)
                {
                    throw new InvalidDataException("Only cancelled or unpaid orders can be repaid.");
                }

                if (_currentUserService.UserId != order.PatientId)
                {
                    throw new UnauthorizedAccessException("You are not authorized to pay this order.");
                }

                var paymentItems = DistributeDiscountToPaymentItems(
                    order.OrderDetails.Select(d => new PaymentEventItem
                    {
                        ServiceName = d.MedicineSnapshot?.MedicineName ?? string.Empty,
                        UnitPrice = Math.Max(0, d.UnitPrice - (d.DiscountAmount ?? 0)),
                        Quantity = d.Quantity
                    }).ToList(),
                    order.FinalPrice);

                var paymentEvent = new CreatePaymentEvent
                {
                    Description = $"ORD {order.Id}",
                    Provider = paymentType,
                    PaymentEventItems = paymentItems
                };

                var createPaymentContract = await _messageBus
                    .RequestAsync<CreatePaymentEvent, CreatePaymentContract>(paymentEvent);

                if (string.IsNullOrWhiteSpace(createPaymentContract.PaymentUrl))
                {
                    throw new InvalidDataException("Failed to retrieve payment URL from payment provider.");
                }

                order.TransactionId = createPaymentContract.TransactionId;

                if (order.OrderStatus == OrderStatusEnum.Cancelled)
                {
                    var decreaseInventoryEvent = new DecreaseInventoryQuantityEvent
                    {
                        DecreaseInventoryQuantityEventItems = _mapper.Map<List<DecreaseInventoryQuantityEventItem>>(
                            order.OrderDetails.Where(d => !string.IsNullOrWhiteSpace(d.SKUCode)))
                    };

                    await _messageBus.RequestAsync<
                        DecreaseInventoryQuantityEvent,
                        DecreaseMedicineQuantityContract>(decreaseInventoryEvent);
                }

                if (order.OrderStatus != OrderStatusEnum.Unpaid)
                {
                    this.UpdateOrderStatus(order, OrderStatusEnum.Unpaid);
                }

                _repository.Update(order);
                await _unitOfWork.SaveChangeAsync();
                await _unitOfWork.CommitTransactionAsync();

                return createPaymentContract.PaymentUrl;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task CancelOrderAsync(int orderId)
        {
            var order = await _repository.GetByIdAsync(orderId, _includes)
                ?? throw new DataNotFoundException(typeof(Order), orderId);

            if (_currentUserService.UserId != order.PatientId)
            {
                throw new UnauthorizedAccessException("You are not authorized to cancel this order.");
            }

            this.UpdateOrderStatus(order, OrderStatusEnum.Cancelled);

            _repository.Update(order);
            await this.RestoreInventoryAsync(order);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task ShippingOrderAsync(int orderId)
        {
            var order = await _repository.GetByIdAsync(orderId)
                ?? throw new DataNotFoundException(typeof(Order), orderId);

            if (order.IsPickupAtStore)
            {
                throw new InvalidDataException("Only delivery orders can be set to Shipping.");
            }

            this.UpdateOrderStatus(order, OrderStatusEnum.Shipping);

            _repository.Update(order);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task CompleteOrderAsync(int orderId)
        {
            var order = await _repository.GetByIdAsync(orderId)
                ?? throw new DataNotFoundException(typeof(Order), orderId);

            if (order.IsPickupAtStore)
            {
                if (order.OrderStatus != OrderStatusEnum.Paid)
                {
                    throw new InvalidDataException("Pickup orders can only be completed from Paid status.");
                }
            }
            else
            {
                if (order.OrderStatus != OrderStatusEnum.Shipping)
                {
                    throw new InvalidDataException("Delivery orders can only be completed from Shipping status.");
                }
            }

            this.UpdateOrderStatus(order, OrderStatusEnum.Completed);

            _repository.Update(order);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task PaidOrderAsync(int orderId)
        {
            var order = await _repository.GetByIdAsync(orderId)
                ?? throw new DataNotFoundException(typeof(Order), orderId);

            this.UpdateOrderStatus(order, OrderStatusEnum.Paid);

            _repository.Update(order);
            await _unitOfWork.SaveChangeAsync();

            if (order.TransactionId != null)
            {
                await _messageBus.PublishAsync(new UpdateOrderStatusEvent
                {
                    TransactionId = order.TransactionId.Value
                });
            }
        }

        public async Task HandlePaymentStatusChangedAsync(Guid transactionId, string paymentStatus)
        {
            var order = await _repository.GetByConditionAsync(x => x.TransactionId == transactionId, _includes)
                ?? throw new DataNotFoundException($"No order found with the specified TransactionId: {transactionId}.");

            if (paymentStatus == DEFAULT_PAYMENT_STATUS_SUCCESS)
            {
                this.UpdateOrderStatus(order, OrderStatusEnum.Paid);
            }
            else
            {
                return;
            }

            _repository.Update(order);
            await _unitOfWork.SaveChangeAsync();
        }
        #endregion

        #region Background Jobs
        public async Task ProcessOverdueOrdersAsync()
        {
            var todayUtc = DateTime.UtcNow;
            var toUpdate = new List<Order>();

            var unpaidOverdue = await _repository.GetAllAsync(
                o => o.OrderStatus == OrderStatusEnum.Unpaid &&
                    o.OrderDate.AddDays(OVERDUE_DAYS) < todayUtc, _includes);

            foreach (var order in unpaidOverdue)
            {
                this.UpdateOrderStatus(order, OrderStatusEnum.Cancelled);
                await this.RestoreInventoryAsync(order);
                toUpdate.Add(order);
            }

            var paidOverdue = await _repository.GetAllAsync(
                o => o.OrderStatus == OrderStatusEnum.Paid &&
                    o.IsPickupAtStore &&
                    o.PaidDate.HasValue &&
                    o.PaidDate.Value.AddDays(OVERDUE_DAYS) < todayUtc, _includes);

            foreach (var order in paidOverdue)
            {
                this.UpdateOrderStatus(order, OrderStatusEnum.Cancelled);
                await this.RestoreInventoryAsync(order);

                toUpdate.Add(order);
            }

            if (toUpdate.Count != 0)
            {
                _repository.UpdateRange(toUpdate);
                await _unitOfWork.SaveChangeAsync();
            }
        }
        #endregion

        #region Private Helpers
        private async Task RestoreInventoryAsync(Order order)
        {
            var eventItems = order.OrderDetails
                .Where(d => !string.IsNullOrWhiteSpace(d.SKUCode))
                .Select(d => new AddQuantityFromCancelOrderEventItem
                {
                    SKUCode = d.SKUCode,
                    Quantity = d.Quantity
                })
                .ToList();

            if (eventItems.Count == 0)
            {
                return;
            }

            await _messageBus.RequestAsync<
                AddQuantityFromCancelOrderEvent,
                AddQuantityFromCancelOrderContract>(
                    new() { Items = eventItems });
        }

        private List<PaymentEventItem> DistributeDiscountToPaymentItems(
            List<PaymentEventItem> paymentItems,
            double targetTotal)
        {
            if (paymentItems.Count == 0)
            {
                return paymentItems;
            }

            var normalizedTargetTotal = Math.Max(0, targetTotal);
            var currentTotal = paymentItems.Sum(item => item.UnitPrice * item.Quantity);

            if (normalizedTargetTotal <= 0 || currentTotal <= 0)
            {
                return paymentItems.Select(item => new PaymentEventItem
                {
                    ServiceName = item.ServiceName,
                    Quantity = item.Quantity,
                    UnitPrice = 0
                }).ToList();
            }

            if (normalizedTargetTotal >= currentTotal)
            {
                return paymentItems;
            }

            var scaledItems = new List<PaymentEventItem>(paymentItems.Count);
            var remainingTargetTotal = normalizedTargetTotal;

            for (var i = 0; i < paymentItems.Count; i++)
            {
                var item = paymentItems[i];
                var quantity = Math.Max(1, item.Quantity);
                var itemTotal = item.UnitPrice * quantity;

                var scaledItemTotal = i == paymentItems.Count - 1
                    ? remainingTargetTotal
                    : Math.Min(itemTotal, normalizedTargetTotal * itemTotal / currentTotal);

                scaledItems.Add(new PaymentEventItem
                {
                    ServiceName = item.ServiceName,
                    Quantity = item.Quantity,
                    UnitPrice = Math.Max(0, scaledItemTotal / quantity)
                });

                remainingTargetTotal = Math.Max(0, remainingTargetTotal - scaledItemTotal);
            }

            return scaledItems;
        }

        private void UpdateOrderStatus(Order order, OrderStatusEnum newStatus)
        {
            var stateMachine = new OrderStatusStateMachine(order);

            if (!stateMachine.CanFire(newStatus))
            {
                throw new InvalidDataException($"Order status cannot be changed to {newStatus}.");
            }

            stateMachine.Fire(newStatus);
        }
        #endregion
    }
}
