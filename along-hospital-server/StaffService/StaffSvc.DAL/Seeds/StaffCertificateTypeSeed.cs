using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;
using StaffSvc.DAL.Models;

namespace StaffSvc.DAL.Seeds
{
    public class StaffCertificateTypeSeed : ISeedBuilder
    {
        public int Priority => 2;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StaffCertificateType>().HasData(
                new StaffCertificateType
                {
                    Id = 1,
                    Name = "BLS Provider",
                    ScopeOfPractice = "Basic Life Support for clinical staff"
                },
                new StaffCertificateType
                {
                    Id = 2,
                    Name = "ACLS Provider",
                    ScopeOfPractice = "Advanced cardiovascular life support"
                },
                new StaffCertificateType
                {
                    Id = 3,
                    Name = "PALS Provider",
                    ScopeOfPractice = "Pediatric advanced life support"
                }
            );

            return modelBuilder;
        }
    }
}
