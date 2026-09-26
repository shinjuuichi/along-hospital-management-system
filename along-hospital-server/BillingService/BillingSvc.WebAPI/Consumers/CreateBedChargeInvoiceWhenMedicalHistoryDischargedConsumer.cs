using AutoMapper;
using BillingSvc.BLL.DTOs.InvoiceDTOs;
using BillingSvc.BLL.Interfaces;
using MassTransit;
using MessageBroker.Events.BillingEvents;
using SharedLibrary.Base.MessageBuses;

namespace BillingSvc.WebAPI.Consumers
{
    public class CreateBedChargeInvoiceWhenMedicalHistoryDischargedConsumer(
        IMapper mapper,
        IInvoiceService invoiceService)
        : RequestConsumer<
            CreateBedChargeInvoiceWhenMedicalHistoryDischargedEvent,
            CreateBedChargeInvoiceWhenMedicalHistoryDischargedContract>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IInvoiceService _invoiceService = invoiceService;

        protected override async Task<CreateBedChargeInvoiceWhenMedicalHistoryDischargedContract> Handle(
            ConsumeContext<CreateBedChargeInvoiceWhenMedicalHistoryDischargedEvent> context)
        {
            var createInvoiceDTO = _mapper.Map<CreateInvoiceDTO>(context.Message);

            await _invoiceService.CreateAsync(createInvoiceDTO);
            return new();
        }
    }
}
