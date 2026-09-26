using StaffSvc.DAL.Enums;
using StaffSvc.DAL.Models;
using Stateless;

namespace StaffSvc.BLL.StateMachines
{
    public class StaffStatusStateMachine
    {
        private readonly Staff _staff;
        private readonly StateMachine<StaffStatusEnum, StaffStatusEnum> _stateMachine;

        public StaffStatusStateMachine(Staff staff)
        {
            _staff = staff;
            _stateMachine = new StateMachine<StaffStatusEnum, StaffStatusEnum>(
                () => _staff.Status,
                s => _staff.Status = s);
            Configure();
        }

        private void Configure()
        {
            _stateMachine.Configure(StaffStatusEnum.Active)
                .Permit(StaffStatusEnum.OnLeave, StaffStatusEnum.OnLeave)
                .Permit(StaffStatusEnum.Suspended, StaffStatusEnum.Suspended)
                .Permit(StaffStatusEnum.Terminated, StaffStatusEnum.Terminated);

            _stateMachine.Configure(StaffStatusEnum.OnLeave)
                .Permit(StaffStatusEnum.Active, StaffStatusEnum.Active)
                .Permit(StaffStatusEnum.Suspended, StaffStatusEnum.Suspended)
                .Permit(StaffStatusEnum.Terminated, StaffStatusEnum.Terminated);

            _stateMachine.Configure(StaffStatusEnum.Suspended)
                .Permit(StaffStatusEnum.Active, StaffStatusEnum.Active);

            _stateMachine.Configure(StaffStatusEnum.Terminated)
                .Ignore(StaffStatusEnum.Terminated);
        }

        public bool CanFire(StaffStatusEnum trigger)
        {
            return _stateMachine.CanFire(trigger);
        }

        public void Fire(StaffStatusEnum trigger)
        {
            _stateMachine.Fire(trigger);
        }
    }
}
