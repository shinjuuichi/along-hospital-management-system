using MedicalOrderSvc.DAL.Enums;
using MedicalOrderSvc.DAL.Models.InfusionMedicalOrders;
using Stateless;

namespace MedicalOrderSvc.BLL.StateMachines
{
    public class InfusionMedicalOrderDetailExecutionStatusStateMachine
    {
        private readonly InfusionMedicalOrderDetail _infusionMedicalOrderDetail;
        private readonly StateMachine<InfusionMedicalOrderDetailExecutionStatusEnum, InfusionMedicalOrderDetailExecutionStatusEnum> _stateMachine;

        public InfusionMedicalOrderDetailExecutionStatusStateMachine(InfusionMedicalOrderDetail infusionMedicalOrderDetail)
        {
            _infusionMedicalOrderDetail = infusionMedicalOrderDetail;
            _stateMachine = new StateMachine<InfusionMedicalOrderDetailExecutionStatusEnum, InfusionMedicalOrderDetailExecutionStatusEnum>(
                () => _infusionMedicalOrderDetail.InfusionMedicalOrderDetailExecutionStatus,
                s => _infusionMedicalOrderDetail.InfusionMedicalOrderDetailExecutionStatus = s);

            Configure();
        }

        private void Configure()
        {
            _stateMachine.Configure(InfusionMedicalOrderDetailExecutionStatusEnum.Pending)
                .Permit(InfusionMedicalOrderDetailExecutionStatusEnum.Completed, InfusionMedicalOrderDetailExecutionStatusEnum.Completed)
                .Permit(InfusionMedicalOrderDetailExecutionStatusEnum.Failed, InfusionMedicalOrderDetailExecutionStatusEnum.Failed);
        }

        public bool CanFire(InfusionMedicalOrderDetailExecutionStatusEnum trigger)
        {
            return _stateMachine.CanFire(trigger);
        }

        public void Fire(InfusionMedicalOrderDetailExecutionStatusEnum trigger)
        {
            _stateMachine.Fire(trigger);
        }
    }
}
