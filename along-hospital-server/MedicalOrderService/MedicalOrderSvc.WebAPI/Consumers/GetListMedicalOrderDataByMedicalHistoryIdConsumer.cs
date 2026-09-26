using AutoMapper;
using MassTransit;
using MedicalOrderSvc.BLL.Interfaces;
using MedicalOrderSvc.BLL.Utils;
using MessageBroker.Contracts.MedicalOrderContracts;
using MessageBroker.Events.MedicalOrderEvents;
using SharedLibrary.Base.MessageBuses;

namespace MedicalOrderSvc.WebAPI.Consumers
{
    public class GetListMedicalOrderDataByMedicalHistoryIdConsumer(
        IMedicalOrderService medicalOrderService,
        IMapper mapper)
            : RequestConsumer<GetListMedicalOrderDataByMedicalHistoryIdEvent, GetListMedicalOrderDataContract>
    {
        private readonly IMedicalOrderService _medicalOrderService = medicalOrderService;
        private readonly IMapper _mapper = mapper;

        protected override async Task<GetListMedicalOrderDataContract> Handle(ConsumeContext<GetListMedicalOrderDataByMedicalHistoryIdEvent> context)
        {
            var medicalHistoryId = context.Message.MedicalHistoryId;

            var medicalOrderDTOs = await _medicalOrderService.GetAllByMedicalHistoryIdAsync(medicalHistoryId);
            List<GetMedicalOrderContract> medicalOrderContracts = _mapper.ConvertMedicalOrderDTOsToContracts(medicalOrderDTOs);

            return new()
            {
                Data = medicalOrderContracts
            };
        }
    }
}
