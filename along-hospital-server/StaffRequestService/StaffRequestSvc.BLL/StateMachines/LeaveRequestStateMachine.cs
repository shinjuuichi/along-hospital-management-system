using StaffRequestSvc.DAL.Enums;
using StaffRequestSvc.DAL.Models;
using Stateless;

namespace StaffRequestSvc.BLL.StateMachines
{
    public class LeaveRequestStateMachine
    {
        private readonly LeaveRequest _entity;
        private readonly StateMachine<RequestStatusEnum, RequestStatusEnum> _stateMachine;
        private readonly int? _currentUserId;

        public LeaveRequestStateMachine(LeaveRequest entity, int? currentUserId = null)
        {
            _entity = entity;
            _currentUserId = currentUserId;
            _stateMachine = new StateMachine<RequestStatusEnum, RequestStatusEnum>(
                () => _entity.Status,
                s => _entity.Status = s);

            this.Configure();
        }

        private void Configure()
        {
            _stateMachine.Configure(RequestStatusEnum.Pending)
                .Permit(RequestStatusEnum.Approved, RequestStatusEnum.Approved)
                .Permit(RequestStatusEnum.Rejected, RequestStatusEnum.Rejected)
                .Permit(RequestStatusEnum.Canceled, RequestStatusEnum.Canceled);

            _stateMachine.Configure(RequestStatusEnum.Approved)
                .OnEntry(() =>
                {
                    _entity.DecidedBy = _currentUserId;
                    _entity.DecidedAt = DateTime.UtcNow;
                });

            _stateMachine.Configure(RequestStatusEnum.Rejected)
                .OnEntry(() =>
                {
                    _entity.DecidedBy = _currentUserId;
                    _entity.DecidedAt = DateTime.UtcNow;
                });
        }

        public bool CanFire(RequestStatusEnum trigger)
        {
            return _stateMachine.CanFire(trigger);
        }

        public void Fire(RequestStatusEnum trigger)
        {
            _stateMachine.Fire(trigger);
        }
    }
}