using MassTransit;
using MedicineSvc.BLL.Interfaces;
using MessageBroker.Events.MedicineEvents;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Commons.Exceptions;

namespace MedicineSvc.WebAPI.Consumers
{
    public class CheckMedicineExistByIdConsumer(
        IMedicineService medicineService)
            : RequestConsumer<CheckMedicineExistByIdEvent, CheckMedicineExistByIdContract>
    {
        private readonly IMedicineService _medicineService = medicineService;

        protected override async Task<CheckMedicineExistByIdContract> Handle(
            ConsumeContext<CheckMedicineExistByIdEvent> context)
        {
            var isExist = await _medicineService.CheckExistByIdAsync(context.Message.MedicineId);

            if (!isExist)
            {
                throw new DataNotFoundException($"No medicine found with ID = {context.Message.MedicineId}");
            }

            return new();
        }
    }
}
