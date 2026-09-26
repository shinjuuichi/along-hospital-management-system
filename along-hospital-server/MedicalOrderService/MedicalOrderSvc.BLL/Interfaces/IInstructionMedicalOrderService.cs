using MedicalOrderSvc.BLL.DTOs.InstructionMedicalOrderDTOs;
using MedicalOrderSvc.BLL.DTOs.MedicalOrderDTOs;
using MedicalOrderSvc.DAL.Enums;

namespace MedicalOrderSvc.BLL.Interfaces
{
    public interface IInstructionMedicalOrderService
    {
        Task<GetMedicalOrderDTO> CreateAsync(CreateInstructionMedicalOrderDTO createDTO);
        Task<GetMedicalOrderDTO> UpdateAsync(string medicalOrderId, UpdateInstructionMedicalOrderDTO updateDTO);
        Task UpdateStatusAsync(string medicalOrderId, InstructionMedicalOrderStatusEnum instructionMedicalOrderStatus);
    }
}
