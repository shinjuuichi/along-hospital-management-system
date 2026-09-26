using MassTransit;
using MedicineSvc.BLL.Interfaces;
using MessageBroker.Events.MedicineEvents;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Commons.Exceptions;

namespace MedicineSvc.WebAPI.Consumers
{
    public class CheckMedicineSKUExistBySKUCodeConsumer(
        IMedicineSKUService medicineSkuService)
            : RequestConsumer<CheckMedicineSKUExistBySKUCodeEvent, CheckMedicineSKUExistBySKUCodeContract>
    {
        private readonly IMedicineSKUService _medicineSkuService = medicineSkuService;

        protected override async Task<CheckMedicineSKUExistBySKUCodeContract> Handle(
            ConsumeContext<CheckMedicineSKUExistBySKUCodeEvent> context)
        {
            if (string.IsNullOrWhiteSpace(context.Message.SKUCode))
            {
                throw new InvalidDataException("SKU code is required.");
            }

            var isExist = await _medicineSkuService.CheckExistBySKUCodeAsync(context.Message.SKUCode);

            if (!isExist)
            {
                throw new DataNotFoundException($"No medicine SKU found with SKUCode = {context.Message.SKUCode}");
            }

            return new();
        }
    }
}
