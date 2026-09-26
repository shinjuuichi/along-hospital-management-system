using AutoMapper;
using BillingSvc.BLL.Interfaces;
using MassTransit;
using MessageBroker.Contracts.BillingContracts;
using MessageBroker.Events.BillingEvents;
using SharedLibrary.Base.MessageBuses;

namespace BillingSvc.WebAPI.Consumers
{
    public class GetListPendingInvoiceDataByClinicalMedicalOrderIdsConsumer(
        IInvoiceService invoiceService,
        IMapper mapper)
            : RequestConsumer<GetListPendingInvoiceDataByClinicalMedicalOrderIdsEvent, GetListInvoiceDataContract>
    {
        private readonly IInvoiceService _invoiceService = invoiceService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetListInvoiceDataContract> Handle(
            ConsumeContext<GetListPendingInvoiceDataByClinicalMedicalOrderIdsEvent> context)
        {
            var clinicalMedicalOrderDict = context.Message.ClinicalMedicalOrderDict;
            var pendingInvoiceDTOs = await _invoiceService.GetOrCreateAllPendingInvoicesByClinicalMedicalOrderIdsAsync(clinicalMedicalOrderDict);

            return new()
            {
                Data = _mapper.Map<List<GetInvoiceContract>>(pendingInvoiceDTOs),
            };
        }
    }
}
