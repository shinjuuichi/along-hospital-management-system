using AppointmentSvc.BLL.DTOs.GetAppointmentDTOs;
using AppointmentSvc.BLL.FilterDTOs;
using SharedLibrary.Commons.Results;

namespace AppointmentSvc.BLL.Interfaces
{
    public interface IAppointmentQueryService
    {
        Task<PaginationResult<GetAppointmentDTO>> GetAllPaginatedAsync(AppointmentFilterDTO appointmentFilterDTO);

        Task<PaginationResult<GetAppointmentDTO>> GetAllTelehealthPaginatedByCurrentDoctorSpecialtyAsync(AppointmentFilterDTO appointmentFilterDTO);

        Task<PaginationResult<GetAppointmentDTO>> GetAllPaginatedByPatientIdAsync(int patientId, AppointmentFilterDTO appointmentFilterDTO);

        Task<GetAppointmentDTO> GetByIdAsync(int id, bool ignoreRequestingValue = false);

        Task<GetAppointmentDTO> GetByTransactionIdAsync(Guid transactionId);

        Task<List<GetAppointmentDTO>> GetAllByIdsAsync(List<int> ids, bool ignoreRequestingValue = false);

        Task<List<GetAppointmentDTO>> GetAllByDateAsync(DateOnly date, bool ignoreRequestingValue = false);

        Task<string> GetPaymentUrlByAppointmentIdAsync(int appointmentId);
    }
}
