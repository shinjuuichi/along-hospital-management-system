using MedicalOrderSvc.BLL.DTOs.ClinicalMedicalOrderDTOs;
using MedicalOrderSvc.BLL.DTOs.MedicalOrderDTOs;
using MedicalOrderSvc.DAL.Enums;

namespace MedicalOrderSvc.BLL.Interfaces
{
    public interface IClinicalMedicalOrderService
    {
        Task<(string MedicalOrderId, GetClinicalMedicalOrderDetailDTO ClinicalMedicalOrderDetail)> GetDetailByIdAsync(string clinicalMedicalOrderDetailId);
        Task<GetMedicalOrderDTO> CreateAsync(CreateClinicalMedicalOrderDTO createDTO);
        Task RequestValueForDTOsAsync(List<GetClinicalMedicalOrderDTO> clinicalMedicalOrderDTOs);
        Task UpdateStatusAsync(string medicalOrderId, ClinicalMedicalOrderStatusEnum clinicalMedicalOrderStatus);
        Task UpdateDetailStatusAsync(
            string medicalOrderId,
            int medicalServiceId,
            ClinicalMedicalOrderDetailStatusEnum clinicalMedicalOrderDetailStatus,
            string? failedReason = null);
    }
}
