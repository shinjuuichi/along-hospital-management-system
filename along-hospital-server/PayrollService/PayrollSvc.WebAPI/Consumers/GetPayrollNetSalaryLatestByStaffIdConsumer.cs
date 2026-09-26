using MassTransit;
using MessageBroker.Contracts.PayrollContracts;
using MessageBroker.Events.PayrollEvents;
using PayrollSvc.BLL.Interfaces;
using SharedLibrary.Base.MessageBuses;

namespace PayrollSvc.WebAPI.Consumers
{
    public class GetPayrollNetSalaryLatestByStaffIdConsumer(
        IPayrollService payrollService)
        : RequestConsumer<GetPayrollNetSalaryLatestByStaffIdEvent, GetPayrollNetSalaryLatestByStaffIDContract>
    {
        private readonly IPayrollService _payrollService = payrollService;

        protected override async Task<GetPayrollNetSalaryLatestByStaffIDContract> Handle(
            ConsumeContext<GetPayrollNetSalaryLatestByStaffIdEvent> context)
        {
            var netSalary = await _payrollService.GetLatestNetSalaryByStaffIdAsync(context.Message.StaffId);

            return new GetPayrollNetSalaryLatestByStaffIDContract
            {
                NetSalary = netSalary
            };
        }
    }
}
