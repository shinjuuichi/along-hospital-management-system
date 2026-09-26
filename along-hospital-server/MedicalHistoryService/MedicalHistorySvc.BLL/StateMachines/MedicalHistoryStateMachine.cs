using MedicalHistorySvc.DAL.Enums;
using MedicalHistorySvc.DAL.Models;
using Stateless;

namespace MedicalHistorySvc.BLL.StateMachines
{
    public class MedicalHistoryStateMachine
    {
        private readonly MedicalHistory _medicalHistory;
        private readonly StateMachine<MedicalHistoryStatusEnum, MedicalHistoryStatusEnum> _stateMachine;

        public MedicalHistoryStateMachine(MedicalHistory medicalHistory)
        {
            _medicalHistory = medicalHistory;
            _stateMachine = new StateMachine<MedicalHistoryStatusEnum, MedicalHistoryStatusEnum>(
                () => _medicalHistory.MedicalHistoryStatus,
                s => _medicalHistory.MedicalHistoryStatus = s);

            Configure();
        }

        private void Configure()
        {
            _stateMachine.Configure(MedicalHistoryStatusEnum.PendingPayment)
                .Permit(MedicalHistoryStatusEnum.Draft, MedicalHistoryStatusEnum.Draft)
                .Permit(MedicalHistoryStatusEnum.Cancelled, MedicalHistoryStatusEnum.Cancelled);

            _stateMachine.Configure(MedicalHistoryStatusEnum.Draft)
                .Permit(MedicalHistoryStatusEnum.Completed, MedicalHistoryStatusEnum.Completed)
                .Permit(MedicalHistoryStatusEnum.Cancelled, MedicalHistoryStatusEnum.Cancelled);

            _stateMachine.Configure(MedicalHistoryStatusEnum.Completed)
                .OnEntry(() =>
                {
                    _medicalHistory.DischargeDate = DateTime.UtcNow;
                });
        }

        public bool CanFire(MedicalHistoryStatusEnum trigger)
        {
            return _stateMachine.CanFire(trigger);
        }

        public void Fire(MedicalHistoryStatusEnum trigger)
        {
            _stateMachine.Fire(trigger);
        }
    }
}
