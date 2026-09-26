using MedicalServiceSvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;
using SharedLibrary.Enums;

namespace MedicalServiceSvc.DAL.Seeds
{
    public class MedicalServiceRoleSeed : ISeedBuilder
    {
        public int Priority => 2;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MedicalServiceRole>().HasData(
                new MedicalServiceRole
                {
                    Role = RoleEnum.Doctor,
                    MedicalServiceId = 1,
                },
                new MedicalServiceRole
                {
                    Role = RoleEnum.Nurse,
                    MedicalServiceId = 1,
                },
                new MedicalServiceRole
                {
                    Role = RoleEnum.Doctor,
                    MedicalServiceId = 2,
                },
                new MedicalServiceRole
                {
                    Role = RoleEnum.Doctor,
                    MedicalServiceId = 3,
                },
                new MedicalServiceRole
                {
                    Role = RoleEnum.Doctor,
                    MedicalServiceId = 4,
                },
                new MedicalServiceRole
                {
                    Role = RoleEnum.Doctor,
                    MedicalServiceId = 5,
                }
            );

            return modelBuilder;
        }
    }
}
