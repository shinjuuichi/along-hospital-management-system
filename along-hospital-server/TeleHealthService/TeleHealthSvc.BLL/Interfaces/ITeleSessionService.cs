using MessageBroker.Events.TeleHealthEvents.TeleSessionEvents;
using TeleHealthSvc.BLL.DTOs.TeleSessionDTOs;

namespace TeleHealthSvc.BLL.Interfaces
{
    public interface ITeleSessionService
    {
        Task CreateTeleSessionByAppointmentDataAsync(CreateTeleSessionRequestDTO createTeleSessionRequestDTO);
        Task<TeleSessionCredentialDTO> GetTeleSessionByTransactionIdAsync(Guid transactionId);
        Task<PatientJoinSessionInfoDTO> GetPatientJoinInfoByTransactionIdAsync(Guid transactionId);
        Task<List<GetTeleSessionDTO>> GetTeleSessionByListAppointmentIdAsync(List<GetTeleSessionByAppointmentIdEventItem> appointments);
    }
}
