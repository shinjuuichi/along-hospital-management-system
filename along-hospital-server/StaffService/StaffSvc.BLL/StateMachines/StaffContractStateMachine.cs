using StaffSvc.DAL.Enums;
using StaffSvc.DAL.Models;
using Stateless;

namespace StaffSvc.BLL.StateMachines
{
    public class StaffContractStateMachine
    {
        private readonly StaffContract _contract;
        private readonly StateMachine<StaffContractStatusEnum, StaffContractStatusEnum> _stateMachine;

        public StaffContractStateMachine(StaffContract contract)
        {
            _contract = contract;
            _stateMachine = new StateMachine<StaffContractStatusEnum, StaffContractStatusEnum>(
                () => _contract.Status,
                s => _contract.Status = s);
            Configure();
        }

        private void Configure()
        {
            _stateMachine.Configure(StaffContractStatusEnum.Active)
                .Permit(StaffContractStatusEnum.Expired, StaffContractStatusEnum.Expired)
                .Permit(StaffContractStatusEnum.Terminated, StaffContractStatusEnum.Terminated);

            _stateMachine.Configure(StaffContractStatusEnum.Expired)
                .Permit(StaffContractStatusEnum.Active, StaffContractStatusEnum.Active);

            _stateMachine.Configure(StaffContractStatusEnum.Terminated)
                .Permit(StaffContractStatusEnum.Active, StaffContractStatusEnum.Active);
        }

        public bool CanFire(StaffContractStatusEnum trigger)
        {
            return _stateMachine.CanFire(trigger);
        }

        public void Fire(StaffContractStatusEnum trigger)
        {
            _stateMachine.Fire(trigger);
        }
    }
}
