using MassTransit;
using MedicalOrderSvc.BLL.Interfaces;
using MedicalOrderSvc.DAL.Enums;
using MessageBroker.Contracts.BillingContracts;
using MessageBroker.Events.MedicalOrderEvents;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Commons.Exceptions;

namespace MedicalOrderSvc.WebAPI.Consumers
{
    public class GetRefundDataByClinicalMedicalOrderDetailIdConsumer(
        IClinicalMedicalOrderService clinicalMedicalOrderService)
        : RequestConsumer<GetRefundDataByClinicalMedicalOrderDetailIdEvent, CreateRefundContract>
    {
        private readonly IClinicalMedicalOrderService _clinicalMedicalOrderService = clinicalMedicalOrderService;

        protected override async Task<CreateRefundContract> Handle(ConsumeContext<GetRefundDataByClinicalMedicalOrderDetailIdEvent> context)
        {
            var clinicalMedicalOrderDetailId = context.Message.ClinicalMedicalOrderDetailId;
            if (string.IsNullOrEmpty(clinicalMedicalOrderDetailId))
            {
                throw new InvalidDataException("Clinical Medical Order Detail Id is required.");
            }

            var (medicalOrderId, clinicalMedicalOrderDetail) = await _clinicalMedicalOrderService.GetDetailByIdAsync(clinicalMedicalOrderDetailId);
            if (clinicalMedicalOrderDetail.ClinicalMedicalOrderDetailStatus != nameof(ClinicalMedicalOrderDetailStatusEnum.Failed))
            {
                throw new DataConflictException("Refund can only be created for failed medical order details.");
            }

            var refundData = new CreateRefundContract
            {
                ClinicalMedicalOrderId = medicalOrderId,
                MedicalServiceId = clinicalMedicalOrderDetail.MedicalServiceId,
                Quantity = clinicalMedicalOrderDetail.Quantity,
                ClinicalMedicalOrderDetailId = clinicalMedicalOrderDetail.Id,
                Reason = "Refund for failed medical order detail."
            };

            return refundData;
        }
    }
}
