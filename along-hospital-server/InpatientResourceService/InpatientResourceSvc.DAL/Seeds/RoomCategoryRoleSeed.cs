using InpatientResourceSvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;
using SharedLibrary.Enums;

namespace InpatientResourceSvc.DAL.Seeds
{
    public class RoomCategoryRoleSeed : ISeedBuilder
    {
        public int Priority => 0;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoomCategoryRole>().HasData(
                new RoomCategoryRole
                {
                    Id = 1,
                    Role = RoleEnum.Patient
                },
                new RoomCategoryRole
                {
                    Id = 2,
                    Role = RoleEnum.Doctor
                },
                new RoomCategoryRole
                {
                    Id = 3,
                    Role = RoleEnum.Nurse
                }
            );

            return modelBuilder;
        }
    }
}
