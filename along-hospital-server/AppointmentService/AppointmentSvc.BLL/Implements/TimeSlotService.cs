using AppointmentSvc.BLL.DTOs.TimeSlotDTOs;
using AppointmentSvc.BLL.Interfaces;
using AppointmentSvc.DAL.Enums;
using AppointmentSvc.DAL.Models;
using AutoMapper;
using MessageBroker.Contracts.WorkScheduleContracts;
using MessageBroker.Events.WorkScheduleEvents;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Base.MessageBuses;
using SharedLibrary.Base.Services;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Enums;
using SharedLibrary.Utils;

namespace AppointmentSvc.BLL.Implements
{
    public class TimeSlotService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IMessageBus messageBus)
            : BaseService<TimeSlot, UpsertTimeSlotDTO, UpsertTimeSlotDTO, GetTimeSlotDTO>(unitOfWork, mapper),
                ITimeSlotService
    {
        private readonly IMessageBus _messageBus = messageBus;
        private readonly IGenericRepository<Appointment> _appointmentRepository = unitOfWork.Repository<Appointment>();

        private const string RoomLocationType = "Room";
        private const string TeleRoomLocationType = "TeleRoom";

        public async Task<List<GetTimeSlotDTO>> GetAvailableTimeSlotsAsync(DateOnly date, int specialtyId)
        {
            var nowTimeZone = DateTime.UtcNow.ConvertTimeToTimeZone();
            var nowDate = DateOnly.FromDateTime(nowTimeZone);
            var nowTime = TimeOnly.FromDateTime(nowTimeZone);

            var timeSlots = await _repository.GetAllAsync(ts => (date > nowDate) || (ts.Time >= nowTime && date == nowDate));

            var timeSlotDTOs = _mapper.Map<List<GetTimeSlotDTO>>(timeSlots)
                .OrderBy(timeSlot => timeSlot.Time)
                .ToList();
            if (timeSlotDTOs.Count > 0)
            {
                await this.GetMaxCapacityForTimeSlotsAsync(date, specialtyId, timeSlotDTOs);
            }

            return timeSlotDTOs;
        }

        public async Task<bool> CheckTimeSlotAvailabilityAsync(ValidateTimeSlotDTO validateTimeSlotDTO)
        {
            var timeSlot = await _repository.GetByIdAsync(validateTimeSlotDTO.TimeSlotId)
                ?? throw new DataNotFoundException(typeof(TimeSlot), validateTimeSlotDTO.TimeSlotId);

            var timeSlotDateTime = validateTimeSlotDTO.Date.ToDateTime(timeSlot.Time);
            var nowTimeZone = DateTime.UtcNow.ConvertTimeToTimeZone();
            if (timeSlotDateTime < nowTimeZone)
            {
                return false;
            }

            var timeSlotDTO = _mapper.Map<GetTimeSlotDTO>(timeSlot);
            await this.GetMaxCapacityForTimeSlotsAsync(validateTimeSlotDTO.Date, validateTimeSlotDTO.SpecialtyId, [timeSlotDTO]);

            return validateTimeSlotDTO.AppointmentMeetingType == nameof(AppointmentMeetingTypeEnum.Telehealth)
                ? timeSlotDTO.AvailableCapacityForTeleHealth > 0
                : timeSlotDTO.AvailableCapacityForInPerson > 0;
        }

        #region Private Helper Methods
        private async Task GetMaxCapacityForTimeSlotsAsync(DateOnly date, int specialtyId, List<GetTimeSlotDTO> timeSlots)
        {
            // Get all work schedules for the given date
            var listWorkScheduleContract = await _messageBus.RequestAsync<
                GetListWorkScheduleByWorkDateEvent,
                GetListWorkScheduleContract>(new() { WorkDate = date });
            var listWorkSchedule = listWorkScheduleContract.Data;

            // Group work schedules by shift and calculate the number of in-person and telehealth doctors for each shift
            var shiftGroups = listWorkSchedule
                .Where(workSchedule => workSchedule.Shift != null)
                .GroupBy(workSchedule => new
                {
                    workSchedule.ShiftId,
                    workSchedule.Shift!.StartTime,
                    workSchedule.Shift.EndTime
                })
                .Select(group => new
                {
                    group.Key.StartTime,
                    group.Key.EndTime,
                    InPersonDoctorCount = group
                        .SelectMany(workSchedule => this.GetInPersonDoctorStaffIdsBySpecialty(workSchedule, specialtyId))
                        .Distinct()
                        .Count(),
                    TeleHealthDoctorCount = group
                        .SelectMany(workSchedule => this.GetTeleHealthDoctorStaffIdsBySpecialty(workSchedule, specialtyId))
                        .Distinct()
                        .Count()
                })
                .ToList();

            // Get all appointments for the given date, specialty, and time slots
            var timeSlotIds = timeSlots.Select(timeSlot => timeSlot.Id).ToList();
            var appointments = await _appointmentRepository.GetAllAsync(appointment =>
                appointment.Date == date
                && appointment.SpecialtyId == specialtyId
                && appointment.AppointmentStatus != AppointmentStatusEnum.Cancelled
                && timeSlotIds.Contains(appointment.TimeSlotId));

            var inPersonAppointmentCountByTimeSlot = appointments
                .Where(appointment => appointment.AppointmentMeetingType == AppointmentMeetingTypeEnum.InPerson)
                .GroupBy(appointment => appointment.TimeSlotId)
                .ToDictionary(group => group.Key, group => group.Count());

            var teleHealthAppointmentCountByTimeSlot = appointments
                .Where(appointment => appointment.AppointmentMeetingType == AppointmentMeetingTypeEnum.Telehealth)
                .GroupBy(appointment => appointment.TimeSlotId)
                .ToDictionary(group => group.Key, group => group.Count());

            /* Calculate the maximum and available capacity for in-person and telehealth appointments for each time slot
             * Maximum Formula:
             * 1. InPerson: CapacityPerDoctor of time slot * number of in-person doctors in the shift that the time slot falls into
             * 2. TeleHealth: number of telehealth doctors in the shift that the time slot falls into
             */
            foreach (var timeSlot in timeSlots)
            {
                var matchedShift = shiftGroups.FirstOrDefault(shift =>
                    timeSlot.Time >= shift.StartTime && timeSlot.Time < shift.EndTime);

                var inPersonDoctorCount = matchedShift?.InPersonDoctorCount ?? 0;
                var teleHealthDoctorCount = matchedShift?.TeleHealthDoctorCount ?? 0;

                timeSlot.MaxCapacityForInPerson = timeSlot.CapacityPerDoctor * inPersonDoctorCount;
                timeSlot.MaxCapacityForTeleHealth = teleHealthDoctorCount;

                var bookedInPersonCount = inPersonAppointmentCountByTimeSlot.TryGetValue(timeSlot.Id, out var inPersonCount)
                    ? inPersonCount
                    : 0;

                var bookedTeleHealthCount = teleHealthAppointmentCountByTimeSlot.TryGetValue(timeSlot.Id, out var teleHealthCount)
                    ? teleHealthCount
                    : 0;

                timeSlot.AvailableCapacityForInPerson = Math.Max(timeSlot.MaxCapacityForInPerson - bookedInPersonCount, 0);
                timeSlot.AvailableCapacityForTeleHealth = Math.Max(timeSlot.MaxCapacityForTeleHealth - bookedTeleHealthCount, 0);
            }
        }

        private IEnumerable<int> GetInPersonDoctorStaffIdsBySpecialty(GetWorkScheduleContract workSchedule, int specialtyId)
        {
            var roomDoctorIds = workSchedule.WorkScheduleAssignments
                .Where(assignment => assignment.Staff != null
                    && string.Equals(assignment.LocationType, RoomLocationType, StringComparison.OrdinalIgnoreCase)
                    && assignment.Staff.SpecialtyId == specialtyId
                    && assignment.Staff.Role == nameof(RoleEnum.Doctor))
                .Select(assignment => assignment.StaffId);

            return roomDoctorIds;
        }

        private IEnumerable<int> GetTeleHealthDoctorStaffIdsBySpecialty(GetWorkScheduleContract workSchedule, int specialtyId)
        {
            var teleDoctorIds = workSchedule.WorkScheduleAssignments
                .Where(assignment => assignment.Staff != null
                    && string.Equals(assignment.LocationType, TeleRoomLocationType, StringComparison.OrdinalIgnoreCase)
                    && assignment.Staff.SpecialtyId == specialtyId
                    && assignment.Staff.Role == nameof(RoleEnum.Doctor))
                .Select(assignment => assignment.StaffId);

            return teleDoctorIds;
        }
        #endregion
    }
}
