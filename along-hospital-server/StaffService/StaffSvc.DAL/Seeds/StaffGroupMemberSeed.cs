using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;
using StaffSvc.DAL.Models;

namespace StaffSvc.DAL.Seeds
{
    public class StaffGroupMemberSeed : ISeedBuilder
    {
        public int Priority => 2;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StaffGroupMember>().HasData(
                // Doctors group (Doctor=4)
                new StaffGroupMember { StaffGroupId = 1, StaffId = 4 },

                // Nurses group (Nurse=5)
                new StaffGroupMember { StaffGroupId = 2, StaffId = 5 },

                // TeleHealth Staff group (Doctor=4, HotlineAgent=12)
                new StaffGroupMember { StaffGroupId = 3, StaffId = 4 },
                new StaffGroupMember { StaffGroupId = 3, StaffId = 12 }
            );

            return modelBuilder;
        }
    }
}
