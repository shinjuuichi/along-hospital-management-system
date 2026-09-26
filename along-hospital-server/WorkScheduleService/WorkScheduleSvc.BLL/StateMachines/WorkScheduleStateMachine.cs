using Stateless;
using WorkScheduleSvc.DAL.Enums;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.BLL.StateMachines
{
    public class WorkScheduleStateMachine
    {
        private readonly WorkSchedule _entity;
        private readonly StateMachine<WorkScheduleStatusEnum, WorkScheduleStatusEnum> _stateMachine;

        public WorkScheduleStateMachine(WorkSchedule entity)
        {
            _entity = entity;
            _stateMachine = new StateMachine<WorkScheduleStatusEnum, WorkScheduleStatusEnum>(
                () => _entity.WorkScheduleStatus,
                s => _entity.WorkScheduleStatus = s);

            this.Configure();
        }

        private void Configure()
        {
            _stateMachine.Configure(WorkScheduleStatusEnum.Draft)
                .Permit(WorkScheduleStatusEnum.Published, WorkScheduleStatusEnum.Published);

            _stateMachine.Configure(WorkScheduleStatusEnum.Published)
                .Permit(WorkScheduleStatusEnum.Locked, WorkScheduleStatusEnum.Locked);

            _stateMachine.Configure(WorkScheduleStatusEnum.Locked)
                .Permit(WorkScheduleStatusEnum.Finalized, WorkScheduleStatusEnum.Finalized);
        }

        public bool CanFire(WorkScheduleStatusEnum trigger)
        {
            return _stateMachine.CanFire(trigger);
        }

        public void Fire(WorkScheduleStatusEnum trigger)
        {
            _stateMachine.Fire(trigger);
        }
    }
}
