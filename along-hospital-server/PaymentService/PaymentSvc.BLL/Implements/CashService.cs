using AutoMapper;
using PaymentSvc.BLL.DTOs.CashDTOs;
using PaymentSvc.BLL.Interfaces;
using PaymentSvc.DAL.Enums;
using PaymentSvc.DAL.Models;
using SharedLibrary.Base.Data.MongoDb;
using SharedLibrary.Commons.Exceptions;

namespace PaymentSvc.BLL.Implements
{
    public class CashService(
        IMongoGenericRepository<CashPayment> cashRepository,
        IMapper mapper,
        IExchangeRateService exchangeRateService) : ICashService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IExchangeRateService _exchangeRateService = exchangeRateService;
        private readonly IMongoGenericRepository<CashPayment> _cashRepository = cashRepository;

        public async Task<GetCashDTO> CreateCashPaymentAsync(CreateCashDTO createCashDTO)
        {
            var cashPayment = _mapper.Map<CashPayment>(createCashDTO);

            cashPayment.ExchangeRate = await _exchangeRateService.GetExchageRate("USD", "VND");
            cashPayment.OriginalAmount = createCashDTO.CashItemDTOs.Sum(i => i.Quantity * i.UnitPrice);
            var exchangeRate = (double)cashPayment.ExchangeRate.Value;
            foreach (var paymentItem in cashPayment.PaymentItems)
            {
                paymentItem.UnitPrice = Math.Round(
                    paymentItem.UnitPrice * exchangeRate,
                    0,
                    MidpointRounding.AwayFromZero);
            }

            cashPayment.FinalAmount = cashPayment.PaymentItems.Sum(i => i.Quantity * i.UnitPrice);

            await _cashRepository.AddAsync(cashPayment);
            return _mapper.Map<GetCashDTO>(cashPayment);
        }

        public async Task UpdatePaymentStatus(Guid transactionId)
        {
            var cashPayment = await _cashRepository.GetByConditionAsync(p => p.TransactionId == transactionId)
                    ?? throw new DataNotFoundException($"Payment with TransactionId {transactionId} not found.");

            cashPayment.Status = PaymentStatusEnum.Success;
            await _cashRepository.UpdateAsync(cashPayment.Id, cashPayment);
        }
    }
}
