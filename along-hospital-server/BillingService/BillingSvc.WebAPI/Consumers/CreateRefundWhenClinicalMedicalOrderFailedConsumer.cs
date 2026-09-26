using AutoMapper;
using BillingSvc.BLL.DTOs.ChargeDTOs;
using BillingSvc.BLL.Interfaces;
using MassTransit;
using MessageBroker.Events.BillingEvents;
using SharedLibrary.Base.MessageBuses;

namespace BillingSvc.WebAPI.Consumers
{
    public class CreateRefundWhenClinicalMedicalOrderFailedConsumer(
        IRefundService refundService,
        IMapper mapper)
            : EventConsumer<CreateRefundWhenClinicalMedicalOrderFailedEvent>
    {
        private readonly IRefundService _refundService = refundService;
        private readonly IMapper _mapper = mapper;

        protected override async Task Handle(ConsumeContext<CreateRefundWhenClinicalMedicalOrderFailedEvent> context)
        {
            var data = context.Message.Data;
            if (data == null)
            {
                return;
            }

            var clinicalMedicalOrderId = data.ClinicalMedicalOrderId;
            if (string.IsNullOrEmpty(clinicalMedicalOrderId))
            {
                return;
            }

            var createChargeDTO = _mapper.Map<CreateChargeDTO>(data);

            await _refundService.CreateByMedicalOrderDataAsync(clinicalMedicalOrderId, createChargeDTO);
        }
    }
}