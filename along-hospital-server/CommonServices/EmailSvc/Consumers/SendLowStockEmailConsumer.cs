using AutoMapper;
using EmailSvc.DTOs;
using EmailSvc.Services;
using MassTransit;
using MessageBroker.Events.SendEmailEvents;
using SharedLibrary.Base.MessageBuses;

namespace EmailSvc.Consumers
{
    public class SendLowStockEmailConsumer(IEmailService _emailService, IMapper _mapper) : EventConsumer<SendLowStockMedicinesEmailEvent>
    {
        protected override async Task Handle(ConsumeContext<SendLowStockMedicinesEmailEvent> ctx)
        {
            var medicines = _mapper.Map<List<SendLowStockMedicineDTO>>(ctx.Message.Data);

            var dto = new SendLowStockMedicinesEmailDTO
            {
                Email = "sinltbce182093@fpt.edu.vn",
                Subject = "Low Stock Alert - Medicine Inventory",
                Medicines = medicines
            };

            await _emailService.SendLowStockEmailAsync(dto);
        }
    }
}
