using StaffSvc.DAL.Enums;
using StaffSvc.DAL.Models;
using Stateless;

namespace StaffSvc.BLL.StateMachines
{
    public class StaffCertificateStateMachine
    {
        private readonly StaffCertificate _certificate;
        private readonly StateMachine<StaffCertificateStatusEnum, StaffCertificateStatusEnum> _stateMachine;

        public StaffCertificateStateMachine(StaffCertificate certificate)
        {
            _certificate = certificate;
            _stateMachine = new StateMachine<StaffCertificateStatusEnum, StaffCertificateStatusEnum>(
                () => _certificate.Status,
                s => _certificate.Status = s);
            Configure();
        }

        private void Configure()
        {
            _stateMachine.Configure(StaffCertificateStatusEnum.Valid)
                .Permit(StaffCertificateStatusEnum.Expired, StaffCertificateStatusEnum.Expired)
                .Permit(StaffCertificateStatusEnum.Suspended, StaffCertificateStatusEnum.Suspended);

            _stateMachine.Configure(StaffCertificateStatusEnum.Suspended)
                .Permit(StaffCertificateStatusEnum.Valid, StaffCertificateStatusEnum.Valid);

            _stateMachine.Configure(StaffCertificateStatusEnum.Expired)
                .Permit(StaffCertificateStatusEnum.Valid, StaffCertificateStatusEnum.Valid);
        }

        public bool CanFire(StaffCertificateStatusEnum trigger)
        {
            return _stateMachine.CanFire(trigger);
        }

        public void Fire(StaffCertificateStatusEnum trigger)
        {
            _stateMachine.Fire(trigger);
        }
    }
}