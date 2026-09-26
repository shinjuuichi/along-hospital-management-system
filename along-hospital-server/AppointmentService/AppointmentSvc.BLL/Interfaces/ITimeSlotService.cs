using AppointmentSvc.BLL.DTOs.TimeSlotDTOs;
using SharedLibrary.Base.Services;

namespace AppointmentSvc.BLL.Interfaces
{
    public interface ITimeSlotService : IBaseCrudService<UpsertTimeSlotDTO, UpsertTimeSlotDTO, GetTimeSlotDTO>
    {
        Task<List<GetTimeSlotDTO>> GetAvailableTimeSlotsAsync(DateOnly date, int specialtyId);

        Task<bool> CheckTimeSlotAvailabilityAsync(ValidateTimeSlotDTO validateTimeSlotDTO);
    }
}
