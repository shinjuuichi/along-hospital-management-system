using QueueSvc.BLL.DTOs;
using QueueSvc.BLL.DTOs.CreateQueueDTOs;
using QueueSvc.BLL.DTOs.GetQueueDTOs;
using QueueSvc.DAL.Enums;

namespace QueueSvc.BLL.Interfaces
{
    public interface IQueueCommandService
    {
        Task<GetQueueDTO> CreateAsync();

        Task<GetQueueDTO> CreateFromQRDataAsync(CreateQueueFromQRDataDTO createDTO);

        Task AssignMedicalHistoryAsync(int queueId, AssignMedicalHistoryToQueueDTO assignDTO);

        Task UpdateStatusAsync(int queueId, QueueStatusEnum status);

        Task RunAppointmentQueueingBackgroundAsync();
    }
}