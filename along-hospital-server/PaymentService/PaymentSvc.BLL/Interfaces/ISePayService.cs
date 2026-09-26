using PaymentSvc.BLL.DTOs.SePayDTOs;

namespace PaymentSvc.BLL.Interfaces
{
    public interface ISePayService
    {
        Task<GetSePayDTO> CreateSePayPaymentAsync(CreateSePayDTO sePayDTO);
        Task<GetSePayDTO> CreateSePayForPayrollAsync(CreateSePayDTO sePayDTO);
        Task ProcessIPNAsync(SePayIPNRequestDTO sePayIPNRequestDTO);
    }
}
