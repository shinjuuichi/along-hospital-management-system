using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.VoucherContracts;
using MessageBroker.Events.VoucherEvents;
using SharedLibrary.Base.MessageBuses;
using VoucherSvc.BLL.DTOs.DiscountDTOs.PreviewListMedicineDiscountDTOs;
using VoucherSvc.BLL.Interfaces;

namespace VoucherSvc.WebAPI.Consumers
{
    public class GetOneMedicineDiscountConsumer(IDiscountService _discountService, IMapper _mapper)
        : RequestConsumer<PreviewMedicineDetailEvent, MedicineDiscountDetailContract>
    {
        protected override async Task<MedicineDiscountDetailContract> Handle(ConsumeContext<PreviewMedicineDetailEvent> context)
        {
            var request = _mapper.Map<PreviewMedicineItemDTO>(context.Message);

            var previewResponse = await _discountService.PreviewMedicineDiscountAsync(request);

            var contract = _mapper.Map<MedicineDiscountDetailContract>(previewResponse);
            return contract;
        }
    }
}
