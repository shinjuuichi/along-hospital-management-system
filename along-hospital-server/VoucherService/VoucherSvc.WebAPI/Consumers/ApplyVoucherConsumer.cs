using AutoMapper;
using MassTransit;
using MessageBroker.Contracts.VoucherContracts;
using MessageBroker.Events.VoucherEvents;
using SharedLibrary.Base.MessageBuses;
using VoucherSvc.BLL.DTOs.DiscountDTOs.ApplyVoucherDTOs;
using VoucherSvc.BLL.Interfaces;

namespace VoucherSvc.WebAPI.Consumers
{
    public class ApplyVoucherConsumer(IDiscountService _discountService, IMapper _mapper)
        : RequestConsumer<ApplyVoucherEvent, ApplyVoucherContract>
    {
        protected override async Task<ApplyVoucherContract> Handle(ConsumeContext<ApplyVoucherEvent> context)
        {
            var request = _mapper.Map<ApplyVoucherRequestDTO>(context.Message);

            var applyVoucherResponseDTO = await _discountService.ApplyVoucherAsync(request);

            var applyVoucherContract = _mapper.Map<ApplyVoucherContract>(applyVoucherResponseDTO);
            return applyVoucherContract;
        }
    }
}
