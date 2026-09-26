using MedicalOrderSvc.DAL.Enums;
using MedicalOrderSvc.DAL.Models.ClinicalMedicalOrders;
using Stateless;

namespace MedicalOrderSvc.BLL.StateMachines
{
    public class ClinicalMedicalOrderStatusStateMachine
    {
        private readonly ClinicalMedicalOrder _clinicalMedicalOrder;
        private readonly StateMachine<ClinicalMedicalOrderStatusEnum, ClinicalMedicalOrderStatusEnum> _stateMachine;

        public ClinicalMedicalOrderStatusStateMachine(ClinicalMedicalOrder clinicalMedicalOrder)
        {
            _clinicalMedicalOrder = clinicalMedicalOrder;
            _stateMachine = new StateMachine<ClinicalMedicalOrderStatusEnum, ClinicalMedicalOrderStatusEnum>(
                () => _clinicalMedicalOrder.ClinicalMedicalOrderStatus,
                s => _clinicalMedicalOrder.ClinicalMedicalOrderStatus = s);

            Configure();
        }

        private void Configure()
        {
            _stateMachine.Configure(ClinicalMedicalOrderStatusEnum.Pending)
                .Permit(ClinicalMedicalOrderStatusEnum.Paid, ClinicalMedicalOrderStatusEnum.Paid)
                .Permit(ClinicalMedicalOrderStatusEnum.Cancelled, ClinicalMedicalOrderStatusEnum.Cancelled);
        }

        public bool CanFire(ClinicalMedicalOrderStatusEnum trigger)
        {
            return _stateMachine.CanFire(trigger);
        }

        public void Fire(ClinicalMedicalOrderStatusEnum trigger)
        {
            _stateMachine.Fire(trigger);
        }
    }
}
