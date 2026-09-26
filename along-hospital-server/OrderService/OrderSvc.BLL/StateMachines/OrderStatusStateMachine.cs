using OrderSvc.DAL.Enums;
using OrderSvc.DAL.Models;
using Stateless;

namespace OrderSvc.BLL.StateMachines
{
    public class OrderStatusStateMachine
    {
        private readonly Order _order;
        private readonly StateMachine<OrderStatusEnum, OrderStatusEnum> _stateMachine;

        public OrderStatusStateMachine(Order order)
        {
            _order = order;
            _stateMachine = new StateMachine<OrderStatusEnum, OrderStatusEnum>(
                () => _order.OrderStatus,
                s => _order.OrderStatus = s);
            Configure();
        }

        private void Configure()
        {
            _stateMachine.Configure(OrderStatusEnum.Unpaid)
                .Permit(OrderStatusEnum.Paid, OrderStatusEnum.Paid)
                .Permit(OrderStatusEnum.Cancelled, OrderStatusEnum.Cancelled);

            _stateMachine.Configure(OrderStatusEnum.Paid)
                .Permit(OrderStatusEnum.Completed, OrderStatusEnum.Completed)
                .Permit(OrderStatusEnum.Shipping, OrderStatusEnum.Shipping)
                .Permit(OrderStatusEnum.Cancelled, OrderStatusEnum.Cancelled);

            _stateMachine.Configure(OrderStatusEnum.Shipping)
                .Permit(OrderStatusEnum.Completed, OrderStatusEnum.Completed);

            _stateMachine.Configure(OrderStatusEnum.Cancelled)
                .Permit(OrderStatusEnum.Unpaid, OrderStatusEnum.Unpaid);

            _stateMachine.Configure(OrderStatusEnum.Completed)
                .OnEntry(() => _order.DeliveryDate = DateTime.UtcNow);

            _stateMachine.Configure(OrderStatusEnum.Paid)
                .OnEntry(() => _order.PaidDate = DateTime.UtcNow);

            _stateMachine.Configure(OrderStatusEnum.Cancelled)
                .OnEntry(() => _order.TransactionId = null);

            _stateMachine.Configure(OrderStatusEnum.Unpaid)
                .OnEntry(() =>
                {
                    _order.PaidDate = null;
                });
        }

        public bool CanFire(OrderStatusEnum trigger) => _stateMachine.CanFire(trigger);

        public void Fire(OrderStatusEnum trigger) => _stateMachine.Fire(trigger);
    }
}
