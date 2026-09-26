using InpatientResourceSvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;

namespace InpatientResourceSvc.DAL.Seeds
{
    public class RoomCategoryRoleMappingSeed : ISeedBuilder
    {
        public int Priority => 1;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoomCategoryRoleMapping>().HasData(
                // Standard Room (Id=1) -> Patient, Nurse
                new RoomCategoryRoleMapping { RoomCategoryRoleId = 1, RoomCategoryId = 1 },
                new RoomCategoryRoleMapping { RoomCategoryRoleId = 3, RoomCategoryId = 1 },

                // VIP Room (Id=2) -> Patient, Doctor, Nurse
                new RoomCategoryRoleMapping { RoomCategoryRoleId = 1, RoomCategoryId = 2 },
                new RoomCategoryRoleMapping { RoomCategoryRoleId = 2, RoomCategoryId = 2 },
                new RoomCategoryRoleMapping { RoomCategoryRoleId = 3, RoomCategoryId = 2 },

                // ICU (Id=3) -> Patient, Doctor, Nurse
                new RoomCategoryRoleMapping { RoomCategoryRoleId = 1, RoomCategoryId = 3 },
                new RoomCategoryRoleMapping { RoomCategoryRoleId = 2, RoomCategoryId = 3 },
                new RoomCategoryRoleMapping { RoomCategoryRoleId = 3, RoomCategoryId = 3 }
            );

            return modelBuilder;
        }
    }
}
