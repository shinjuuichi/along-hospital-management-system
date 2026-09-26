using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.VoucherContracts;
using MessageBroker.Events.VoucherEvents;
using SharedLibrary.Base.MessageBuses;
using VoucherSvc.BLL.DTOs.DiscountDTOs.PreviewVoucherDTOs;
using VoucherSvc.BLL.Interfaces;

namespace VoucherSvc.WebAPI.Consumers
{
    public class PreviewVoucherConsumer(IDiscountService _discountService, IMapper _mapper)
        : RequestConsumer<PreviewVoucherEvent, PreviewVoucherContract>
    {
        protected override async Task<PreviewVoucherContract> Handle(ConsumeContext<PreviewVoucherEvent> context)
        {
            var request = _mapper.Map<PreviewVoucherRequestDTO>(context.Message);

            var previewResponse = await _discountService.PreviewVoucherAsync(request);

            var contract = _mapper.Map<PreviewVoucherContract>(previewResponse);
            return contract;
        }
    }
}
