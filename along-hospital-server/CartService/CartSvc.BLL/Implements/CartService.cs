using AutoMapper;
using CartSvc.BLL.DTOs;
using CartSvc.BLL.Interfaces;
using CartSvc.DAL.Models;
using MessageBroker.Contracts.MedicineContracts;
using MessageBroker.Contracts.OrderContracts;
using MessageBroker.Contracts.VoucherContracts;
using MessageBroker.Events.MedicineEvents;
using MessageBroker.Events.OrderEvents;
using MessageBroker.Events.VoucherEvents;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Services.Interfaces;

namespace CartSvc.BLL.Implements
{
    public class CartService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMessageBus messageBus,
        ICurrentUserService currentUserService)
            : ICartService
    {
        private readonly IMessageBus _messageBus = messageBus;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        private readonly IGenericRepository<Cart> _cartRepository = unitOfWork.Repository<Cart>();
        private readonly string[] _includes = [nameof(Cart.CartDetails)];

        public async Task CreateAsync(int patientId)
        {
            var existingCart = await _cartRepository.GetByConditionAsync(c => c.PatientId == patientId);
            if (existingCart != null)
            {
                return;
            }

            var cart = new Cart { PatientId = patientId };

            await _cartRepository.AddAsync(cart);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task AddToCartAsync(UpsertCartDetailDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.SKUCode))
            {
                throw new InvalidDataException("SKU code is required.");
            }

            await _messageBus.RequestAsync<CheckMedicineSKUExistBySKUCodeEvent, CheckMedicineSKUExistBySKUCodeContract>(
                    new() { SKUCode = dto.SKUCode });

            var cart = await this.GetCartByCurrentUserAsync();
            var existingDetail = cart.CartDetails.FirstOrDefault(cd => cd.SKUCode == dto.SKUCode);

            if (existingDetail != null)
            {
                existingDetail.Quantity += dto.Quantity;
            }
            else
            {
                var newDetail = new CartDetail
                {
                    SKUCode = dto.SKUCode,
                    Quantity = dto.Quantity,
                };
                cart.CartDetails.Add(newDetail);
            }

            _cartRepository.Update(cart);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task UpdateDetailAsync(UpsertCartDetailDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.SKUCode))
            {
                throw new InvalidDataException("SKU code is required.");
            }

            if (dto.Quantity <= 0)
            {
                await this.DeleteDetailAsync(dto.SKUCode);
                return;
            }

            await _messageBus.RequestAsync<CheckMedicineSKUExistBySKUCodeEvent, CheckMedicineSKUExistBySKUCodeContract>(
                   new() { SKUCode = dto.SKUCode });

            var cart = await this.GetCartByCurrentUserAsync();
            var existingDetail = cart.CartDetails.FirstOrDefault(cd => cd.SKUCode == dto.SKUCode)
                ?? throw new DataNotFoundException("This item does not exist in your cart.");
            existingDetail.Quantity = dto.Quantity;

            _cartRepository.Update(cart);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task DeleteDetailAsync(string skuCode)
        {
            var cart = await this.GetCartByCurrentUserAsync();
            var existingDetail = cart.CartDetails.FirstOrDefault(cd => cd.SKUCode == skuCode)
                ?? throw new DataNotFoundException("This item does not exist in your cart.");

            cart.CartDetails.Remove(existingDetail);
            _cartRepository.Update(cart);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<GetCartDTO> GetCurrentUserCartAsync()
        {
            var cart = await this.GetCartByCurrentUserAsync();
            var skuCodes = cart.CartDetails
                .Where(d => !string.IsNullOrWhiteSpace(d.SKUCode))
                .Select(d => d.SKUCode)
                .Distinct()
                .ToList();

            List<GetMedicineSKUContract> medicineSKUDataList = [];
            if (skuCodes.Count > 0)
            {
                var getMedicineSKUData = await _messageBus
                    .RequestAsync<GetPublicMedicineSKUsBySKUCodesEvent, GetListMedicineSKUDataContract>(
                        new() { SKUCodes = skuCodes });

                medicineSKUDataList = getMedicineSKUData.Data;
            }

            Dictionary<string, double> discountDict = [];
            if (medicineSKUDataList.Count > 0)
            {
                var previewListMedicineDiscountContract = await _messageBus
                    .RequestAsync<PreviewListMedicineDiscountEvent, PreviewListMedicineDiscountContract>(
                    new PreviewListMedicineDiscountEvent
                    {
                        Medicines = _mapper.Map<List<PreviewMedicineDetailEvent>>(medicineSKUDataList)
                    });

                foreach (var discount in previewListMedicineDiscountContract.MedicineDiscounts)
                {
                    if (string.IsNullOrWhiteSpace(discount.SKUCode))
                    {
                        continue;
                    }

                    discountDict[discount.SKUCode] = discount.FinalPrice;
                }
            }

            var medicineSKUDtos = _mapper.Map<List<GetMedicineSKUDTO>>(medicineSKUDataList);

            var medicineDict = medicineSKUDtos.ToDictionary(m => m.SKUCode ?? string.Empty);

            var cartDto = _mapper.Map<GetCartDTO>(cart);
            foreach (var detail in cartDto.CartDetails)
            {
                if (detail.SKUCode == null)
                {
                    throw new InvalidDataException("SKU code is required for cart details.");
                }

                if (medicineDict.TryGetValue(detail.SKUCode, out var medicineSku))
                {
                    detail.MedicineSKU = medicineSku;
                    detail.OriginPrice = medicineSku.Price;
                }

                if (discountDict.TryGetValue(detail.SKUCode, out var discountedPrice))
                {
                    detail.DiscountPrice = discountedPrice;
                }
            }

            return cartDto;
        }

        public async Task<GetPaymentUrlDTO> CheckoutAsync(CheckoutDTO checkoutDTO)
        {
            var selectedSKUCodes = checkoutDTO.SelectedSKUCodes;
            if (selectedSKUCodes.Count == 0)
            {
                throw new ValidationFailureException("No SKU selected");
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var cart = await this.GetCartByCurrentUserAsync();
                var cartDetails = cart.CartDetails.ToList();

                var cartSkuSet = cartDetails.Select(d => d.SKUCode).ToHashSet();
                var invalidSkus = selectedSKUCodes.Where(s => !cartSkuSet.Contains(s)).ToList();

                if (invalidSkus.Count > 0)
                {
                    throw new DataNotFoundException("SKU not found in cart.");
                }

                var selectedDetails = cartDetails
                    .Where(d => selectedSKUCodes.Contains(d.SKUCode))
                    .ToList();

                var cartMedicineEventItems = selectedDetails.Select(i => new CartMedicineEventItem
                {
                    SKUCode = i.SKUCode,
                    Quantity = i.Quantity,
                }).ToList();

                var createOrderByCartDataEvent = new CreateOrderByCartDataEvent
                {
                    UserId = _currentUserService.UserId,
                    VoucherCode = checkoutDTO.VoucherCode,
                    Description = checkoutDTO.Description,
                    PaymentType = checkoutDTO.PaymentType,
                    CartMedicineEventItems = cartMedicineEventItems,
                    IsPickupAtStore = checkoutDTO.IsPickupAtStore,
                };

                var createOrderContract = await _messageBus.RequestAsync<CreateOrderByCartDataEvent, CreateOrderByCartDataContract>(createOrderByCartDataEvent);

                foreach (var detail in selectedDetails)
                {
                    cart.CartDetails.Remove(detail);
                }

                _cartRepository.Update(cart);
                await _unitOfWork.SaveChangeAsync();

                await _unitOfWork.CommitTransactionAsync();

                return new GetPaymentUrlDTO { PaymentUrl = createOrderContract.PaymentUrl };
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        private async Task<Cart> GetCartByCurrentUserAsync()
        {
            var userId = _currentUserService.UserId;
            var cart = await _cartRepository.GetByConditionAsync(c => c.PatientId == userId, _includes);
            if (cart == null)
            {
                var newCart = new Cart { PatientId = userId };
                cart = await _cartRepository.AddAsync(newCart);
                await _unitOfWork.SaveChangeAsync();
            }

            return cart;
        }
    }
}