using MassTransit;
using MessageBroker.Events.VoucherEvents;
using SharedLibrary.Base.MessageBuses;
using VoucherSvc.BLL.Interfaces;

namespace VoucherSvc.WebAPI.Consumers
{
    public class CreateVoucherForNewPatientConsumer(IPatientVoucherService _voucherCollectionService)
        : EventConsumer<CreateVoucherForNewPatientIdEvent>
    {
        protected override async Task Handle(ConsumeContext<CreateVoucherForNewPatientIdEvent> context)
        {
            var patientId = context.Message.UserId;
            await _voucherCollectionService.CollectVoucherForNewPatientAsync(patientId);
        }
    }
}

