using StaffRequestSvc.DAL.Enums;
using StaffRequestSvc.DAL.Models;
using Stateless;

namespace StaffRequestSvc.BLL.StateMachines
{
    public class SalaryAdvanceStateMachine
    {
        private readonly SalaryAdvance _entity;
        private readonly StateMachine<SalaryAdvanceStatusEnum, SalaryAdvanceStatusEnum> _stateMachine;
        private readonly int? _currentUserId;

        public SalaryAdvanceStateMachine(SalaryAdvance entity, int? currentUserId = null)
        {
            _entity = entity;
            _currentUserId = currentUserId;
            _stateMachine = new StateMachine<SalaryAdvanceStatusEnum, SalaryAdvanceStatusEnum>(
                () => _entity.Status,
                s => _entity.Status = s);

            this.Configure();
        }

        private void Configure()
        {
            _stateMachine.Configure(SalaryAdvanceStatusEnum.Pending)
                .Permit(SalaryAdvanceStatusEnum.Approved, SalaryAdvanceStatusEnum.Approved)
                .Permit(SalaryAdvanceStatusEnum.Rejected, SalaryAdvanceStatusEnum.Rejected)
                .Permit(SalaryAdvanceStatusEnum.Cancelled, SalaryAdvanceStatusEnum.Cancelled);

            _stateMachine.Configure(SalaryAdvanceStatusEnum.Approved)
                .Permit(SalaryAdvanceStatusEnum.Disbursed, SalaryAdvanceStatusEnum.Disbursed)
                .OnEntry(() =>
                {
                    _entity.DecidedBy = _currentUserId;
                    _entity.DecidedAt = DateTime.UtcNow;
                });

            _stateMachine.Configure(SalaryAdvanceStatusEnum.Rejected)
                .OnEntry(() =>
                {
                    _entity.DecidedBy = _currentUserId;
                    _entity.DecidedAt = DateTime.UtcNow;
                });
        }

        public bool CanFire(SalaryAdvanceStatusEnum trigger)
        {
            return _stateMachine.CanFire(trigger);
        }

        public void Fire(SalaryAdvanceStatusEnum trigger)
        {
            _stateMachine.Fire(trigger);
        }
    }
}