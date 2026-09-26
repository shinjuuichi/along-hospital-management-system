using MassTransit;
using MedicalOrderSvc.BLL.Interfaces;
using MedicalOrderSvc.DAL.Enums;
using MessageBroker.Events.MedicalOrderEvents;
using SharedLibrary.Base.MessageBuses;

namespace MedicalOrderSvc.WebAPI.Consumers
{
    public class UpdateMedicalOrderWhenInvoiceCompletedConsumer(
        IClinicalMedicalOrderService clinicalMedicalOrderService)
            : EventConsumer<UpdateMedicalOrderWhenInvoiceCompletedEvent>
    {
        private readonly IClinicalMedicalOrderService _clinicalMedicalOrderService = clinicalMedicalOrderService;

        protected override async Task Handle(ConsumeContext<UpdateMedicalOrderWhenInvoiceCompletedEvent> context)
        {
            var id = context.Message.Id;
            if (string.IsNullOrEmpty(id))
            {
                return;
            }

            await _clinicalMedicalOrderService.UpdateStatusAsync(id, ClinicalMedicalOrderStatusEnum.Paid);
        }
    }
}
