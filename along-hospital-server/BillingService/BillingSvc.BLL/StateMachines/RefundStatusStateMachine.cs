using BillingSvc.DAL.Enums;
using BillingSvc.DAL.Models;
using Stateless;

namespace BillingSvc.BLL.StateMachines
{
    public class RefundStatusStateMachine
    {
        private readonly Refund _refund;
        private readonly int _currentUserId;
        private readonly StateMachine<RefundStatusEnum, RefundStatusEnum> _stateMachine;

        public RefundStatusStateMachine(Refund refund, int currentUserId)
        {
            _refund = refund;
            _currentUserId = currentUserId;
            _stateMachine = new StateMachine<RefundStatusEnum, RefundStatusEnum>(
                () => _refund.RefundStatus,
                s => _refund.RefundStatus = s);

            Configure();
        }

        private void Configure()
        {
            _stateMachine.Configure(RefundStatusEnum.Pending)
                    .Permit(RefundStatusEnum.Approved, RefundStatusEnum.Approved)
                    .Permit(RefundStatusEnum.Cancelled, RefundStatusEnum.Cancelled);

            _stateMachine.Configure(RefundStatusEnum.Approved)
                .OnEntry(() =>
                {
                    _refund.ApprovedBy = _currentUserId;
                    _refund.ApprovalDate = DateTime.UtcNow;
                });
        }

        public bool CanFire(RefundStatusEnum trigger)
        {
            return _stateMachine.CanFire(trigger);
        }

        public void Fire(RefundStatusEnum trigger)
        {
            _stateMachine.Fire(trigger);
        }
    }
}
