using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.DAL.Seeds
{
    public class WorkScheduleTemplateAssignmentForStaffTeleRoomSeed : ISeedBuilder
    {
        public int Priority => 2;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkScheduleTemplateAssignmentForStaffTeleRoom>().HasData(
                new WorkScheduleTemplateAssignmentForStaffTeleRoom
                {
                    WorkScheduleTemplateId = 1,
                    ShiftId = 1,
                    StaffId = 14,
                    TeleRoomId = 1
                },
                new WorkScheduleTemplateAssignmentForStaffTeleRoom
                {
                    WorkScheduleTemplateId = 1,
                    ShiftId = 2,
                    StaffId = 4,
                    TeleRoomId = 4
                },
                new WorkScheduleTemplateAssignmentForStaffTeleRoom
                {
                    WorkScheduleTemplateId = 2,
                    ShiftId = 1,
                    StaffId = 14,
                    TeleRoomId = 1
                },
                new WorkScheduleTemplateAssignmentForStaffTeleRoom
                {
                    WorkScheduleTemplateId = 2,
                    ShiftId = 2,
                    StaffId = 4,
                    TeleRoomId = 4
                }
            );

            return modelBuilder;
        }
    }
}
