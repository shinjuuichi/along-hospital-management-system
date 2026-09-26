using AutoMapper;
using MessageBroker.Events.PaymentEvents;
using Net.payOS;
using Net.payOS.Types;
using PaymentSvc.BLL.DTOs.PayOSDTOs;
using PaymentSvc.BLL.Interfaces;
using PaymentSvc.DAL.Enums;
using PaymentSvc.DAL.Models;
using SharedLibrary.Base.Data.MongoDb;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Commons;
using SharedLibrary.Commons.Exceptions;

namespace PaymentSvc.BLL.Implements
{
    public class PayOSService(
        PayOS payOS,
        IMongoGenericRepository<PayOSPayment> payOSRepository,
        IMapper mapper,
        IMessageBus messageBus,
        IExchangeRateService exchangeRateService,
        AppConfiguration appConfiguration) : IPayOSService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IMessageBus _messageBus = messageBus;
        private readonly PayOS _payOS = payOS;
        private readonly AppConfiguration _appConfiguration = appConfiguration;
        private readonly IExchangeRateService _exchangeRateService = exchangeRateService;
        private readonly IMongoGenericRepository<PayOSPayment> _payOSRepisitory = payOSRepository;

        private const string PaymentCancelPath = "/payment/cancel";
        private const string PaymentReturnPath = "/payment/return";

        private const string PayOSBaseUrl = "https://pay.payos.vn/web/";

        public async Task<GetPayOSDTO> CreatePayOSPaymentAsync(CreatePayOSDTO createPayOSDTO)
        {
            var cancelUrl = $"{_appConfiguration.UrlsConfig.FrontendUrl}{PaymentCancelPath}";
            var returnUrl = $"{_appConfiguration.UrlsConfig.FrontendUrl}{PaymentReturnPath}";

            var payOSPayment = _mapper.Map<PayOSPayment>(createPayOSDTO);

            payOSPayment.ProviderOrderCode = Random.Shared.NextInt64(1000000000, 9999999999);
            payOSPayment.ExchangeRate = await _exchangeRateService.GetExchageRate("USD", "VND");
            payOSPayment.OriginalAmount = createPayOSDTO.PayOsItemDTOs.Sum(i => i.Quantity * i.UnitPrice);
            var exchangeRate = (double)payOSPayment.ExchangeRate.Value;
            foreach (var paymentItem in payOSPayment.PaymentItems)
            {
                paymentItem.UnitPrice = Math.Round(
                    paymentItem.UnitPrice * exchangeRate,
                    0,
                    MidpointRounding.AwayFromZero);
            }

            payOSPayment.FinalAmount = payOSPayment.PaymentItems.Sum(i => i.Quantity * i.UnitPrice);

            var itemDatas = payOSPayment.PaymentItems
                .Select(item => new ItemData(item.ServiceName, item.Quantity, (int)item.UnitPrice))
                .ToList();
            PaymentData paymentData = new(payOSPayment.ProviderOrderCode, (int)payOSPayment.FinalAmount, createPayOSDTO.Description!, itemDatas, cancelUrl, returnUrl);
            CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);

            var paymentUrl = createPayment.checkoutUrl;
            if (string.IsNullOrEmpty(paymentUrl))
            {
                throw new InvalidOperationException("Failed to create payment link.");
            }

            await _payOSRepisitory.AddAsync(payOSPayment);

            return new GetPayOSDTO { PaymentUrl = paymentUrl, TransactionId = payOSPayment.TransactionId };
        }

        public async Task UpdatePaymentStatus(long providerOrderCode, PaymentStatusEnum status)
        {
            var payOSPayment = await _payOSRepisitory.GetByConditionAsync(p => p.ProviderOrderCode == providerOrderCode)
                ?? throw new DataNotFoundException("Payment not found");

            payOSPayment.Status = status;
            await _payOSRepisitory.UpdateAsync(payOSPayment.Id, payOSPayment);

            var paymentStatusChangedEvent = _mapper.Map<PaymentStatusChangedEvent>(payOSPayment);
            await _messageBus.PublishAsync(paymentStatusChangedEvent);
        }

        public async Task<string> GetPaymentUrlByTransactionIdAsync(Guid? transactionId)
        {
            var payOSPayment = await _payOSRepisitory.GetByConditionAsync(p => p.TransactionId == transactionId)
                    ?? throw new DataNotFoundException($"Payment with TransactionId {transactionId} not found.");

            if (payOSPayment.Status != PaymentStatusEnum.Pending)
            {
                throw new InvalidOperationException("Payment is not in a pending state.");
            }

            var paymentInformation = await _payOS.getPaymentLinkInformation(payOSPayment.ProviderOrderCode);

            return PayOSBaseUrl + paymentInformation.id;
        }
    }
}