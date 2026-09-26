using MedicalOrderSvc.BLL.DTOs.InfusionMedicalOrderDTOs;
using MedicalOrderSvc.BLL.DTOs.MedicalOrderDTOs;
using MedicalOrderSvc.DAL.Enums;

namespace MedicalOrderSvc.BLL.Interfaces
{
    public interface IInfusionMedicalOrderService
    {
        Task<GetMedicalOrderDTO> CreateAsync(CreateInfusionMedicalOrderDTO createDTO);
        Task UpdateDetailStatusAsync(string medicalOrderId, int medicineId, InfusionMedicalOrderDetailExecutionStatusEnum infusionMedicalOrderDetailExecutionStatus);
    }
}
