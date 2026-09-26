using MedicalOrderSvc.DAL.Enums;
using MedicalOrderSvc.DAL.Models.InstructionMedicalOrders;
using Stateless;

namespace MedicalOrderSvc.BLL.StateMachines
{
    public class InstructionMedicalOrderStatusStateMachine
    {
        private readonly InstructionMedicalOrder _instructionMedicalOrder;
        private readonly StateMachine<InstructionMedicalOrderStatusEnum, InstructionMedicalOrderStatusEnum> _stateMachine;

        public InstructionMedicalOrderStatusStateMachine(InstructionMedicalOrder instructionMedicalOrder)
        {
            _instructionMedicalOrder = instructionMedicalOrder;
            _stateMachine = new StateMachine<InstructionMedicalOrderStatusEnum, InstructionMedicalOrderStatusEnum>(
                () => _instructionMedicalOrder.InstructionMedicalOrderStatus,
                s => _instructionMedicalOrder.InstructionMedicalOrderStatus = s);

            Configure();
        }

        private void Configure()
        {
            _stateMachine.Configure(InstructionMedicalOrderStatusEnum.Draft)
                .Permit(InstructionMedicalOrderStatusEnum.Issued, InstructionMedicalOrderStatusEnum.Issued)
                .Permit(InstructionMedicalOrderStatusEnum.Cancelled, InstructionMedicalOrderStatusEnum.Cancelled);

            _stateMachine.Configure(InstructionMedicalOrderStatusEnum.Issued)
                .Permit(InstructionMedicalOrderStatusEnum.Cancelled, InstructionMedicalOrderStatusEnum.Cancelled);
        }

        public bool CanFire(InstructionMedicalOrderStatusEnum trigger)
        {
            return _stateMachine.CanFire(trigger);
        }

        public void Fire(InstructionMedicalOrderStatusEnum trigger)
        {
            _stateMachine.Fire(trigger);
        }
    }
}
