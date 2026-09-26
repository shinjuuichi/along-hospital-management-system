using AutoMapper;
using BillingSvc.BLL.Interfaces;
using MassTransit;
using MessageBroker.Contracts.BillingContracts;
using MessageBroker.Events.BillingEvents;
using SharedLibrary.Base.MessageBuses;

namespace BillingSvc.WebAPI.Consumers
{
    public class GetListInvoiceDataByMedicalHistoryIdConsumer(
        IInvoiceService invoiceService,
        IMapper mapper)
            : RequestConsumer<GetListInvoiceDataByMedicalHistoryIdEvent, GetListInvoiceDataContract>
    {
        private readonly IInvoiceService _invoiceService = invoiceService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetListInvoiceDataContract> Handle(ConsumeContext<GetListInvoiceDataByMedicalHistoryIdEvent> context)
        {
            var medicalHistoryId = context.Message.MedicalHistoryId;

            var invoiceDTOs = await _invoiceService.GetAllByMedicalHistoryIdAsync(medicalHistoryId);
            var invoiceContracts = _mapper.Map<List<GetInvoiceContract>>(invoiceDTOs);

            return new GetListInvoiceDataContract
            {
                Data = invoiceContracts
            };
        }
    }
}