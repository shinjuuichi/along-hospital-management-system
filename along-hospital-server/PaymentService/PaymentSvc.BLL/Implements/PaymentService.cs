using AutoMapper;
using MessageBroker.Events.PaymentEvents;
using PaymentSvc.BLL.Interfaces;
using PaymentSvc.DAL.Enums;
using PaymentSvc.DAL.Models;
using SharedLibrary.Base.Data.MongoDb;
using SharedLibrary.Base.MessageBuses;

namespace PaymentSvc.BLL.Implements
{
    public class PaymentService(
        IMongoGenericRepository<Payment> paymentRepository,
        IMessageBus messageBus,
        IMapper mapper) : IPaymentService
    {
        private readonly IMessageBus _messageBus = messageBus;
        private readonly IMapper _mapper = mapper;
        private readonly IMongoGenericRepository<Payment> _paymentRepository = paymentRepository;

        public async Task UpdatePaymentExpiredAsync()
        {
            var payments = await _paymentRepository.GetAllAsync(p => p.Status == PaymentStatusEnum.Pending);

            foreach (var payment in payments)
            {
                payment.Status = PaymentStatusEnum.Failed;
                await _paymentRepository.UpdateAsync(payment.Id, payment);

                var paymentStatusChangedEvent = _mapper.Map<PaymentStatusChangedEvent>(payment);
                await _messageBus.PublishAsync(paymentStatusChangedEvent);
            }
        }
    }
}