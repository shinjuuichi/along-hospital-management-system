using AppointmentSvc.BLL.DTOs;
using AppointmentSvc.DAL.Enums;

namespace AppointmentSvc.BLL.Interfaces
{
    public interface IAppointmentCommandService
    {
        Task<CreateAppointmentResponseDTO> CreateAsync(CreateAppointmentRequestDTO createAppointmentDTO);

        Task UpdateStatusAsync(int appointmentId, AppointmentStatusEnum appointmentStatus);

        Task UpdateListStatusAsync(List<int> appointmentIds, AppointmentStatusEnum appointmentStatus);

        Task HandlePaymentStatusChangedAsync(PaymentStatusChangedDTO paymentStatusChangedDTO);
    }
}
