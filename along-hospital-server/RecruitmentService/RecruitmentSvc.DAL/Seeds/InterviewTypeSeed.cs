using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;
using RecruitmentSvc.DAL.Models;

namespace RecruitmentSvc.DAL.Seeds
{
    public class InterviewTypeSeed : ISeedBuilder
    {
        public int Priority => 1;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var seedCreationDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<InterviewType>().HasData(
                new InterviewType
                {
                    Id = 1,
                    Name = "Phone Interview",
                    Description = "Initial screening via phone call",
                    CreationDate = seedCreationDate
                },
                new InterviewType
                {
                    Id = 2,
                    Name = "Technical Interview",
                    Description = "Technical skills assessment with technical team",
                    CreationDate = seedCreationDate
                },
                new InterviewType
                {
                    Id = 3,
                    Name = "Final Interview",
                    Description = "Final interview with management",
                    CreationDate = seedCreationDate
                }
            );

            return modelBuilder;
        }
    }
}
