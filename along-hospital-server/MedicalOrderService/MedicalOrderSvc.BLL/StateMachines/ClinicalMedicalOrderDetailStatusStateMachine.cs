using MedicalOrderSvc.DAL.Enums;
using MedicalOrderSvc.DAL.Models.ClinicalMedicalOrders;
using Stateless;

namespace MedicalOrderSvc.BLL.StateMachines
{
    public class ClinicalMedicalOrderDetailStatusStateMachine
    {
        private readonly ClinicalMedicalOrderDetail _clinicalMedicalOrderDetail;
        private readonly StateMachine<ClinicalMedicalOrderDetailStatusEnum, ClinicalMedicalOrderDetailStatusEnum> _stateMachine;

        public ClinicalMedicalOrderDetailStatusStateMachine(ClinicalMedicalOrderDetail clinicalMedicalOrderDetail)
        {
            _clinicalMedicalOrderDetail = clinicalMedicalOrderDetail;
            _stateMachine = new StateMachine<ClinicalMedicalOrderDetailStatusEnum, ClinicalMedicalOrderDetailStatusEnum>(
                () => _clinicalMedicalOrderDetail.ClinicalMedicalOrderDetailStatus,
                s => _clinicalMedicalOrderDetail.ClinicalMedicalOrderDetailStatus = s);

            Configure();
        }

        private void Configure()
        {
            _stateMachine.Configure(ClinicalMedicalOrderDetailStatusEnum.Pending)
                .Permit(ClinicalMedicalOrderDetailStatusEnum.Completed, ClinicalMedicalOrderDetailStatusEnum.Completed)
                .Permit(ClinicalMedicalOrderDetailStatusEnum.Failed, ClinicalMedicalOrderDetailStatusEnum.Failed);
        }

        public bool CanFire(ClinicalMedicalOrderDetailStatusEnum trigger)
        {
            return _stateMachine.CanFire(trigger);
        }

        public void Fire(ClinicalMedicalOrderDetailStatusEnum trigger)
        {
            _stateMachine.Fire(trigger);
        }
    }
}
