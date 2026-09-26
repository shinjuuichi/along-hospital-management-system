using AppointmentSvc.BLL.DTOs.StatisticsDTOs;
using AppointmentSvc.BLL.Interfaces;
using AppointmentSvc.DAL.Enums;
using AppointmentSvc.DAL.Models;
using AutoMapper;
using MessageBroker.Contracts.AppointmentContracts;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Utils;

namespace AppointmentSvc.BLL.Implements
{
    public class AppointmentStatisticsService(IUnitOfWork unitOfWork, IMapper mapper) : IAppointmentStatisticsService
    {
        private readonly IGenericRepository<Appointment> _appointmentRepository = unitOfWork.Repository<Appointment>();
        private readonly IMapper _mapper = mapper;

        public async Task<GetAppointmentStatisticsByDateRangeContract> GetStatisticsByDateRangeAsync(DateOnly fromDate, DateOnly toDate)
        {
            var appointments = await _appointmentRepository.GetAllAsync(
                appointment =>
                    appointment.Date >= fromDate &&
                    appointment.Date <= toDate);

            var appointmentsByDay = appointments
                .GroupBy(appointment => appointment.Date)
                .ToDictionary(group => group.Key, group => group.Count());

            var statistics = new AppointmentStatisticsDTO
            {
                Appointments = appointments.Count,
                CompletedAppointments = appointments.Count(appointment => appointment.AppointmentStatus == AppointmentStatusEnum.Completed),
                CancelledAppointments = appointments.Count(appointment => appointment.AppointmentStatus == AppointmentStatusEnum.Cancelled),
                AppointmentStatus = StatisticsContractBuilder.CreateDistribution(
                    appointments.GroupBy(appointment => appointment.AppointmentStatus)
                        .ToDictionary(group => group.Key, group => group.Count())),
                AppointmentMeetingType = StatisticsContractBuilder.CreateDistribution(
                    appointments.GroupBy(appointment => appointment.AppointmentMeetingType)
                        .ToDictionary(group => group.Key, group => group.Count())),
                AppointmentPaymentStatus = StatisticsContractBuilder.CreateDistribution(
                    appointments.GroupBy(appointment => appointment.AppointmentPaymentStatus)
                        .ToDictionary(group => group.Key, group => group.Count())),
                AppointmentsOverTime = StatisticsContractBuilder.CreateSingleSeriesChart(fromDate, toDate, "appointments", appointmentsByDay)
            };

            return _mapper.Map<GetAppointmentStatisticsByDateRangeContract>(statistics);
        }
    }
}
