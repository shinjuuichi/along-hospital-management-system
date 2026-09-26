using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.DAL.Seeds
{
    public class WorkScheduleTemplateAssignmentForStaffRoomSeed : ISeedBuilder
    {
        public int Priority => 2;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkScheduleTemplateAssignmentForStaffRoom>().HasData(
                new WorkScheduleTemplateAssignmentForStaffRoom
                {
                    WorkScheduleTemplateId = 1,
                    ShiftId = 1,
                    StaffId = 5,
                    RoomId = 1
                },
                new WorkScheduleTemplateAssignmentForStaffRoom
                {
                    WorkScheduleTemplateId = 1,
                    ShiftId = 1,
                    StaffId = 15,
                    RoomId = 2
                },
                new WorkScheduleTemplateAssignmentForStaffRoom
                {
                    WorkScheduleTemplateId = 2,
                    ShiftId = 1,
                    StaffId = 5,
                    RoomId = 1
                },
                new WorkScheduleTemplateAssignmentForStaffRoom
                {
                    WorkScheduleTemplateId = 2,
                    ShiftId = 1,
                    StaffId = 15,
                    RoomId = 2
                }
            );

            return modelBuilder;
        }
    }
}
