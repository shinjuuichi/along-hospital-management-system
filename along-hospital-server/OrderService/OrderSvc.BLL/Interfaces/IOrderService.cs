using OrderSvc.BLL.DTOs;
using SharedLibrary.Base.Services;
using SharedLibrary.Commons.Results;

namespace OrderSvc.BLL.Interfaces
{
    public interface IOrderService : IBaseCrudService<CreateOrderDTO, NotUpdateOrderDTO, GetOrderDTO>
    {
        Task<PaginationResult<GetOrderDTO>> GetAllPaginatedByUserIdAsync(OrderFilterDTO orderFilterDTO);
        Task<GetOrderDTO?> GetOrderByCurrentUserAsync(int orderId);
        Task CancelOrderAsync(int orderId);
        Task ShippingOrderAsync(int orderId);
        Task CompleteOrderAsync(int orderId);
        Task PaidOrderAsync(int orderId);
        Task HandlePaymentStatusChangedAsync(Guid transactionId, string paymentStatus);
        Task<string> RepayOrderAsync(int orderId, string paymentType);
        Task ProcessOverdueOrdersAsync();
    }
}