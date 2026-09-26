using PayrollSvc.DAL.Enums;
using PayrollSvc.DAL.Models;
using Stateless;

namespace PayrollSvc.BLL.StateMachines
{
    public class PayrollStatusStateMachine
    {
        private readonly Payroll _payroll;
        private readonly StateMachine<PayrollStatusEnum, PayrollStatusEnum> _stateMachine;

        public PayrollStatusStateMachine(Payroll payroll)
        {
            _payroll = payroll;
            _stateMachine = new StateMachine<PayrollStatusEnum, PayrollStatusEnum>(
                () => _payroll.Status,
                status => _payroll.Status = status);

            this.Configure();
        }

        private void Configure()
        {
            _stateMachine.Configure(PayrollStatusEnum.Draft)
                .Permit(PayrollStatusEnum.Pending, PayrollStatusEnum.Pending)
                .Permit(PayrollStatusEnum.Terminated, PayrollStatusEnum.Terminated);

            _stateMachine.Configure(PayrollStatusEnum.Pending)
                .Permit(PayrollStatusEnum.Approved, PayrollStatusEnum.Approved)
                .Permit(PayrollStatusEnum.Terminated, PayrollStatusEnum.Terminated);

            _stateMachine.Configure(PayrollStatusEnum.Approved)
                .Permit(PayrollStatusEnum.Paid, PayrollStatusEnum.Paid)
                .Permit(PayrollStatusEnum.Terminated, PayrollStatusEnum.Terminated);
        }

        public bool CanFire(PayrollStatusEnum trigger)
        {
            return _stateMachine.CanFire(trigger);
        }

        public void Fire(PayrollStatusEnum trigger)
        {
            _stateMachine.Fire(trigger);
        }
    }
}
