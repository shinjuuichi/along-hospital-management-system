using AutoMapper;
using MessageBroker.Events.PaymentEvents;
using PaymentSvc.BLL.DTOs.SePayDTOs;
using PaymentSvc.BLL.Interfaces;
using PaymentSvc.BLL.Utils;
using PaymentSvc.DAL.Enums;
using PaymentSvc.DAL.Models;
using SharedLibrary.Base.Data.MongoDb;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Commons;
using SharedLibrary.Commons.Exceptions;

namespace PaymentSvc.BLL.Implements
{
    public class SePayService(
        AppConfiguration appConfiguration,
        IMapper mapper,
        IExchangeRateService exchangeRateService,
        IMongoGenericRepository<SePayPayment> sePayRepository,
        IMessageBus messageBus) : ISePayService
    {
        private readonly AppConfiguration _appConfiguration = appConfiguration;
        private readonly IMapper _mapper = mapper;
        private readonly IExchangeRateService _exchangeRateService = exchangeRateService;
        private readonly IMongoGenericRepository<SePayPayment> _sePayRepository = sePayRepository;
        private readonly IMessageBus _messageBus = messageBus;

        public async Task<GetSePayDTO> CreateSePayPaymentAsync(CreateSePayDTO sePayDTO)
        {
            var baseUrl = _appConfiguration.SePayConfig.BaseUrl;
            var va = _appConfiguration.SePayConfig.VA;
            var bankName = _appConfiguration.SePayConfig.BankName;

            var sePay = _mapper.Map<SePayPayment>(sePayDTO);

            sePay.ExchangeRate = await _exchangeRateService.GetExchageRate("USD", "VND");
            sePay.OriginalAmount = sePayDTO.SePayItemDTOs.Sum(i => i.Quantity * i.UnitPrice);

            var exchangeRate = (double)sePay.ExchangeRate.Value;
            foreach (var paymentItem in sePay.PaymentItems)
            {
                paymentItem.UnitPrice = Math.Round(
                    paymentItem.UnitPrice * exchangeRate,
                    0,
                    MidpointRounding.AwayFromZero);
            }

            sePay.FinalAmount = sePay.PaymentItems.Sum(i => i.Quantity * i.UnitPrice);
            sePay.Description += $" ORDER{sePay.TransactionId}";

            var sePayUrl = $"{baseUrl}/img?acc={va}&template=compact&bank={bankName}&amount={sePay.FinalAmount}&des={sePay.Description}";
            await _sePayRepository.AddAsync(sePay);

            return new GetSePayDTO { PaymentUrl = sePayUrl, TransactionId = sePay.TransactionId };
        }

        public async Task<GetSePayDTO> CreateSePayForPayrollAsync(CreateSePayDTO sePayDTO)
        {
            if (string.IsNullOrWhiteSpace(sePayDTO.AccountNumber))
            {
                throw new InvalidDataException("Account number is required for payroll payment.");
            }

            if (string.IsNullOrWhiteSpace(sePayDTO.BankCode))
            {
                throw new InvalidDataException("Bank code is required for payroll payment.");
            }

            var baseUrl = _appConfiguration.SePayConfig.BaseUrl;
            var va = sePayDTO.AccountNumber;
            var bankName = sePayDTO.BankCode;

            var sePay = _mapper.Map<SePayPayment>(sePayDTO);

            sePay.OriginalAmount = sePayDTO.SePayItemDTOs.Sum(i => i.Quantity * i.UnitPrice);
            sePay.Description += $" PAYSLIP{sePay.TransactionId}";
            sePay.FinalAmount = sePay.OriginalAmount;

            var sePayUrl = $"{baseUrl}/img?acc={va}&template=compact&bank={bankName}&amount={sePay.FinalAmount}&des={sePay.Description}";
            await _sePayRepository.AddAsync(sePay);

            return new GetSePayDTO { PaymentUrl = sePayUrl, TransactionId = sePay.TransactionId };
        }

        public async Task ProcessIPNAsync(SePayIPNRequestDTO sePayIPNRequestDTO)
        {
            GuidUtil.TryGetTransactionIdFromOrderContent(sePayIPNRequestDTO.Content!, out var transationIdFromContent);

            var sePayPayment = await _sePayRepository.GetByConditionAsync(p => p.TransactionId == transationIdFromContent)
                ?? throw new DataNotFoundException($"SePay payment with ID {sePayIPNRequestDTO.Content} not found.");

            sePayPayment.Status = PaymentStatusEnum.Success;
            await _sePayRepository.UpdateAsync(sePayPayment.Id, sePayPayment);

            var paymentStatusChangedEvent = _mapper.Map<PaymentStatusChangedEvent>(sePayPayment);
            await _messageBus.PublishAsync(paymentStatusChangedEvent);
        }
    }
}
