using MassTransit;
using MessageBroker.Events.StaffRequestEvents;
using SharedLibrary.Base.MessageBuses;
using StaffRequestSvc.BLL.Interfaces;

namespace StaffRequestSvc.WebAPI.Consumers
{
    public class MarkSalaryAdvanceDisbursedByPayrollConsumer(
        ISalaryAdvanceService salaryAdvanceService)
        : EventConsumer<MarkSalaryAdvanceDisbursedByPayrollEvent>
    {
        private readonly ISalaryAdvanceService _salaryAdvanceService = salaryAdvanceService;

        protected override async Task Handle(ConsumeContext<MarkSalaryAdvanceDisbursedByPayrollEvent> context)
        {
            await _salaryAdvanceService.MarkDisbursedByPayrollAsync(
                context.Message.StaffId,
                context.Message.PayrollId);
        }
    }
}
