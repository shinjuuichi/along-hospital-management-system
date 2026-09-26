using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.VoucherContracts;
using MessageBroker.Events.VoucherEvents;
using SharedLibrary.Base.MessageBuses;
using VoucherSvc.BLL.DTOs.DiscountDTOs.PreviewListMedicineDiscountDTOs;
using VoucherSvc.BLL.Interfaces;

namespace VoucherSvc.WebAPI.Consumers
{
    public class PreviewListMedicineDiscountConsumer(IDiscountService _discountService, IMapper _mapper)
        : RequestConsumer<PreviewListMedicineDiscountEvent, PreviewListMedicineDiscountContract>
    {
        protected override async Task<PreviewListMedicineDiscountContract> Handle(ConsumeContext<PreviewListMedicineDiscountEvent> context)
        {
            var request = _mapper.Map<PreviewListMedicineDiscountRequestDTO>(context.Message);

            var previewResponse = await _discountService.PreviewListMedicineDiscountAsync(request);

            var contract = _mapper.Map<PreviewListMedicineDiscountContract>(previewResponse);
            return contract;
        }
    }
}
