using InpatientResourceSvc.DAL.Enums;
using InpatientResourceSvc.DAL.Models;
using Stateless;

namespace InpatientResourceSvc.BLL.StateMachines
{
    public class BedOccupancyStateMachine
    {
        private readonly BedOccupancy _bedOccupancy;
        private readonly StateMachine<OccupancyStatusEnum, OccupancyStatusEnum> _stateMachine;

        public BedOccupancyStateMachine(BedOccupancy bedOccupancy)
        {
            _bedOccupancy = bedOccupancy;
            _stateMachine = new StateMachine<OccupancyStatusEnum, OccupancyStatusEnum>(
                () => _bedOccupancy.OccupancyStatus,
                status => _bedOccupancy.OccupancyStatus = status);

            this.Configure();
        }

        private void Configure()
        {
            _stateMachine.Configure(OccupancyStatusEnum.Active)
                .Permit(OccupancyStatusEnum.Transferred, OccupancyStatusEnum.Transferred)
                .Permit(OccupancyStatusEnum.Discharged, OccupancyStatusEnum.Discharged);

            _stateMachine.Configure(OccupancyStatusEnum.Transferred);
            _stateMachine.Configure(OccupancyStatusEnum.Discharged);
        }

        public bool CanFire(OccupancyStatusEnum trigger)
        {
            return _stateMachine.CanFire(trigger);
        }

        public void Fire(OccupancyStatusEnum trigger)
        {
            _stateMachine.Fire(trigger);
        }
    }
}