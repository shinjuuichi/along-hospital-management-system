using AppointmentSvc.DAL.Enums;
using AppointmentSvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;

namespace AppointmentSvc.DAL.Seeds
{
    public class AppointmentSeed : ISeedBuilder
    {
        public int Priority => 1;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var defaultDate = new DateOnly(2025, 1, 1);
            var defaultDateTime = defaultDate.ToDateTime(new TimeOnly(0, 0));

            modelBuilder.Entity<Appointment>().HasData(
                new Appointment
                {
                    Id = 1,
                    Date = defaultDate,
                    Purpose = "Follow-up Consultation",
                    AppointmentStatus = AppointmentStatusEnum.Completed,
                    AppointmentMeetingType = AppointmentMeetingTypeEnum.Telehealth,
                    AppointmentPaymentStatus = AppointmentPaymentStatusEnum.Completed,
                    CompletedDate = new DateTime(2025, 1, 1, 10, 30, 0),
                    MedicalHistoryId = 2,
                    SpecialtyId = 3,
                    PatientId = 3,
                    TimeSlotId = 8,
                    TimeSlotSnapshot = new TimeSlotSnapshot
                    {
                        Id = 8,
                        Time = new TimeOnly(10, 30)
                    },
                    CreationDate = defaultDateTime
                },
                new Appointment
                {
                    Id = 2,
                    Date = defaultDate.AddDays(1),
                    Purpose = "Neurology Checkup",
                    AppointmentStatus = AppointmentStatusEnum.Cancelled,
                    AppointmentMeetingType = AppointmentMeetingTypeEnum.Telehealth,
                    AppointmentPaymentStatus = AppointmentPaymentStatusEnum.Failed,
                    CancelledDate = new DateTime(2025, 1, 2, 13, 30, 0),
                    SpecialtyId = 2,
                    PatientId = 3,
                    TimeSlotId = 10,
                    TimeSlotSnapshot = new TimeSlotSnapshot
                    {
                        Id = 10,
                        Time = new TimeOnly(13, 30)
                    },
                    CreationDate = defaultDateTime
                }
            );

            return modelBuilder;
        }
    }
}
