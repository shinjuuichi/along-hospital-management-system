using Microsoft.EntityFrameworkCore;
using RecruitmentSvc.DAL.Enums;
using RecruitmentSvc.DAL.Models;
using SharedLibrary.Base.Data;

namespace RecruitmentSvc.DAL.Seeds
{
    public class InterviewSeed : ISeedBuilder
    {
        public int Priority => 3;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var seedCreationDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<Interview>().HasData(
                new Interview
                {
                    Id = 1,
                    InterviewDate = new DateTime(2025, 2, 10, 10, 0, 0),
                    Result = InterviewResultEnum.Passed,
                    Note = "Excellent technical skills and good communication. Recommended for hire.",
                    JobApplicationId = 1,
                    InterviewTypeId = 2,
                    CreationDate = seedCreationDate
                },
                new Interview
                {
                    Id = 2,
                    InterviewDate = new DateTime(2025, 2, 15, 14, 0, 0),
                    Result = InterviewResultEnum.Pending,
                    Note = "Scheduled for technical interview round 2.",
                    JobApplicationId = 2,
                    InterviewTypeId = 1,
                    CreationDate = seedCreationDate
                },
                new Interview
                {
                    Id = 3,
                    InterviewDate = new DateTime(2025, 2, 5, 9, 0, 0),
                    Result = InterviewResultEnum.Passed,
                    Note = "Good experience in nursing. Passed both phone and technical interviews.",
                    JobApplicationId = 3,
                    InterviewTypeId = 3,
                    CreationDate = seedCreationDate
                },
                new Interview
                {
                    Id = 4,
                    InterviewDate = new DateTime(2029, 2, 20, 11, 0, 0),
                    Result = InterviewResultEnum.Pending,
                    Note = "Awaiting final interview result.",
                    JobApplicationId = 4,
                    InterviewTypeId = 2,
                    CreationDate = seedCreationDate
                }
            );

            return modelBuilder;
        }
    }
}
