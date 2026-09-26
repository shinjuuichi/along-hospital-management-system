using AutoMapper;
using MassTransit;
using MessageBroker.Events.PayrollEvents;
using PayrollSvc.BLL.DTOs.PayrollDTOs;
using PayrollSvc.BLL.Interfaces;
using SharedLibrary.Base.MessageBuses;

namespace PayrollSvc.WebAPI.Consumers
{
    public class CreatePayrollConsumer(IPayrollService payrollService, IMapper mapper) : EventConsumer<CreatePayrollEvent>
    {
        private readonly IPayrollService _payrollService = payrollService;
        private readonly IMapper _mapper = mapper;

        protected override async Task Handle(ConsumeContext<CreatePayrollEvent> context)
        {
            var createPayrollDTO = _mapper.Map<CreatePayrollDTO>(context.Message);
            await _payrollService.CreateAsync(createPayrollDTO);
        }
    }
}