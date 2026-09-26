using MassTransit;
using MessageBroker.Events.StaffEvents.SpecialtyEvents;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Commons.Exceptions;
using StaffSvc.BLL.Interfaces;
using StaffSvc.DAL.Models;

namespace StaffSvc.WebAPI.Consumers.SpecialtyConsumers
{
    public class CheckSpecialtyExistByIdConsumer(
        ISpecialtyService specialtyService)
        : RequestConsumer<CheckSpecialtyExistByIdEvent, CheckSpecialtyExistByIdContract>
    {
        private readonly ISpecialtyService _specialtyService = specialtyService;

        protected override async Task<CheckSpecialtyExistByIdContract> Handle(ConsumeContext<CheckSpecialtyExistByIdEvent> context)
        {
            var specialtyId = context.Message.SpecialtyId;
            var specialtyExist = await _specialtyService.CheckSpecialtyExistByIdAsync(specialtyId);
            if (!specialtyExist)
            {
                throw new DataNotFoundException(typeof(Specialty), specialtyId);
            }

            return new CheckSpecialtyExistByIdContract
            {
                IsSuccess = true,
            };
        }
    }
}