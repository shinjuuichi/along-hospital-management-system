using AppointmentSvc.DAL.Enums;
using AppointmentSvc.DAL.Models;
using Stateless;

namespace AppointmentSvc.BLL.StateMachines
{
    public class AppointmentStatusStateMachine
    {
        private readonly Appointment _appointment;
        private readonly StateMachine<AppointmentStatusEnum, AppointmentStatusEnum> _stateMachine;

        public AppointmentStatusStateMachine(Appointment appointment)
        {
            _appointment = appointment;
            _stateMachine = new StateMachine<AppointmentStatusEnum, AppointmentStatusEnum>(
                () => _appointment.AppointmentStatus,
                s => _appointment.AppointmentStatus = s);
            Configure();
        }

        private void Configure()
        {
            _stateMachine.Configure(AppointmentStatusEnum.Scheduled)
                    .PermitIf(AppointmentStatusEnum.Completed, AppointmentStatusEnum.Completed, CanComplete)
                    .Permit(AppointmentStatusEnum.Cancelled, AppointmentStatusEnum.Cancelled);

            _stateMachine.Configure(AppointmentStatusEnum.Completed)
                .OnEntry(() =>
                {
                    _appointment.CompletedDate = DateTime.UtcNow;
                });

            _stateMachine.Configure(AppointmentStatusEnum.Cancelled)
                .OnEntry(() =>
                {
                    _appointment.CancelledDate = DateTime.UtcNow;
                    if (_appointment.AppointmentPaymentStatus == AppointmentPaymentStatusEnum.Pending)
                    {
                        _appointment.AppointmentPaymentStatus = AppointmentPaymentStatusEnum.Failed;
                        _appointment.TransactionId = null;
                    }
                });
        }

        public bool CanFire(AppointmentStatusEnum trigger)
        {
            return _stateMachine.CanFire(trigger);
        }

        public void Fire(AppointmentStatusEnum trigger)
        {
            _stateMachine.Fire(trigger);
        }

        private bool CanComplete()
        {
            if (_appointment.TimeSlot == null)
            {
                return false;
            }

            var now = DateTime.UtcNow;
            DateTime appointmentDateTime = _appointment.Date.ToDateTime(_appointment.TimeSlot.Time);
            return appointmentDateTime <= now;
        }
    }
}