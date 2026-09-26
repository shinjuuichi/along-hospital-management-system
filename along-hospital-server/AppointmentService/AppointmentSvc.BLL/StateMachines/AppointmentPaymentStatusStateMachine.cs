using AppointmentSvc.DAL.Enums;
using AppointmentSvc.DAL.Models;
using Stateless;

namespace AppointmentSvc.BLL.StateMachines
{
    public class AppointmentPaymentStatusStateMachine
    {
        private readonly Appointment _appointment;
        private readonly StateMachine<AppointmentPaymentStatusEnum, AppointmentPaymentStatusEnum> _stateMachine;

        public AppointmentPaymentStatusStateMachine(Appointment appointment)
        {
            _appointment = appointment;
            _stateMachine = new StateMachine<AppointmentPaymentStatusEnum, AppointmentPaymentStatusEnum>(
                () => _appointment.AppointmentPaymentStatus,
                s => _appointment.AppointmentPaymentStatus = s);
            Configure();
        }

        private void Configure()
        {
            _stateMachine.Configure(AppointmentPaymentStatusEnum.Pending)
                    .Permit(AppointmentPaymentStatusEnum.Completed, AppointmentPaymentStatusEnum.Completed)
                    .Permit(AppointmentPaymentStatusEnum.Failed, AppointmentPaymentStatusEnum.Failed);

            _stateMachine.Configure(AppointmentPaymentStatusEnum.Failed)
                    .Permit(AppointmentPaymentStatusEnum.Pending, AppointmentPaymentStatusEnum.Pending);
        }

        public bool CanFire(AppointmentPaymentStatusEnum trigger)
        {
            return _stateMachine.CanFire(trigger);
        }

        public void Fire(AppointmentPaymentStatusEnum trigger)
        {
            _stateMachine.Fire(trigger);
        }
    }
}