using MassTransit;
using MedicineSvc.BLL.Interfaces;
using MessageBroker.Events.MedicineEvents;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Commons.Exceptions;

namespace MedicineSvc.WebAPI.Consumers
{
    public class CheckMedicineSKUExistByIdConsumer(
        IMedicineSKUService medicineSkuService)
            : RequestConsumer<CheckMedicineSKUExistByIdEvent, CheckMedicineSKUExistByIdContract>
    {
        private readonly IMedicineSKUService _medicineSkuService = medicineSkuService;

        protected override async Task<CheckMedicineSKUExistByIdContract> Handle(
            ConsumeContext<CheckMedicineSKUExistByIdEvent> context)
        {
            var isExist = await _medicineSkuService.CheckExistByIdAsync(context.Message.MedicineSKUId);

            if (!isExist)
            {
                throw new DataNotFoundException($"No medicine SKU found with ID = {context.Message.MedicineSKUId}");
            }

            return new();
        }
    }
}