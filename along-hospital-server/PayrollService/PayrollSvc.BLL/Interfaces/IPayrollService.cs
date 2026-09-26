using PayrollSvc.BLL.DTOs.PayrollDTOs;
using PayrollSvc.DAL.Enums;
using SharedLibrary.Base.Services;

namespace PayrollSvc.BLL.Interfaces
{
    public interface IPayrollService : IBaseCrudService<CreatePayrollDTO, UpdatePayrollDTO, GetPayrollDTO>
    {
        Task<double> GetLatestNetSalaryByStaffIdAsync(int staffId);
        Task HandlePaymentStatusChangedAsync(PaymentStatusChangedDTO paymentStatusChangedDTO);
        Task TerminatePayrollsByStaffIdsAsync(List<int> staffIds);
        Task UpdatePayrollStatusAsync(List<int> ids, PayrollStatusEnum payrollStatus);
    }
}