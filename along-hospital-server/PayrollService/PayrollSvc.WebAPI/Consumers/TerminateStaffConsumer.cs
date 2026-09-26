using MassTransit;
using MessageBroker.Events.StaffEvents;
using PayrollSvc.BLL.Interfaces;
using SharedLibrary.Base.MessageBuses;

namespace PayrollSvc.WebAPI.Consumers
{
    public class TerminateStaffConsumer(IPayrollService payrollService)
        : EventConsumer<TerminateStaffEvent>
    {
        private readonly IPayrollService _payrollService = payrollService;

        protected override async Task Handle(ConsumeContext<TerminateStaffEvent> context)
        {
            await _payrollService.TerminatePayrollsByStaffIdsAsync(context.Message.StaffIds);
        }
    }
}
