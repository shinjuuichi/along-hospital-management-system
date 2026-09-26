using BillingSvc.DAL.Enums;
using BillingSvc.DAL.Models;
using Stateless;

namespace BillingSvc.BLL.StateMachines
{
    public class InvoiceStatusStateMachine
    {
        private readonly Invoice _invoice;
        private readonly StateMachine<InvoiceStatusEnum, InvoiceStatusEnum> _stateMachine;

        public InvoiceStatusStateMachine(Invoice invoice)
        {
            _invoice = invoice;
            _stateMachine = new StateMachine<InvoiceStatusEnum, InvoiceStatusEnum>(
                () => _invoice.InvoiceStatus,
                s => _invoice.InvoiceStatus = s);

            Configure();
        }

        private void Configure()
        {
            _stateMachine.Configure(InvoiceStatusEnum.Pending)
                    .Permit(InvoiceStatusEnum.Completed, InvoiceStatusEnum.Completed)
                    .Permit(InvoiceStatusEnum.Cancelled, InvoiceStatusEnum.Cancelled);

            _stateMachine.Configure(InvoiceStatusEnum.Completed)
                .OnEntry(() =>
                {
                    _invoice.PaymentDate = DateTime.UtcNow;
                });
        }

        public bool CanFire(InvoiceStatusEnum trigger)
        {
            return _stateMachine.CanFire(trigger);
        }

        public void Fire(InvoiceStatusEnum trigger)
        {
            _stateMachine.Fire(trigger);
        }
    }
}