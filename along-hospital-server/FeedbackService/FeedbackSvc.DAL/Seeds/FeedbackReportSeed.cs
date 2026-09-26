using FeedbackSvc.DAL.Enums;
using FeedbackSvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;

namespace FeedbackSvc.DAL.Seeds
{
    public class FeedbackReportSeed : ISeedBuilder
    {
        public int Priority => 2;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var seedCreationDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<FeedbackReport>().HasData(
                new FeedbackReport
                {
                    Id = 1,
                    FeedbackId = 1,
                    Reason = "Content appears overly promotional and should be reviewed by moderation.",
                    Status = FeedbackReportStatusEnum.Pending,
                    CreationDate = seedCreationDate.AddDays(1),
                    CreatedBy = 39
                },
                new FeedbackReport
                {
                    Id = 2,
                    FeedbackId = 2,
                    Reason = "Feedback mentions product performance but lacks enough detail, needs moderation review.",
                    Status = FeedbackReportStatusEnum.Resolved,
                    CreationDate = seedCreationDate.AddDays(2),
                    CreatedBy = 40
                },
                new FeedbackReport
                {
                    Id = 3,
                    FeedbackId = 3,
                    Reason = "Feedback describes side effects and was reviewed as acceptable health-related sharing.",
                    Status = FeedbackReportStatusEnum.Rejected,
                    CreationDate = seedCreationDate.AddDays(3),
                    CreatedBy = 41
                },
                new FeedbackReport
                {
                    Id = 4,
                    FeedbackId = 7,
                    Reason = "Report requests moderation to verify whether the content is too generic.",
                    Status = FeedbackReportStatusEnum.Pending,
                    CreationDate = seedCreationDate.AddDays(7),
                    CreatedBy = 42
                },
                new FeedbackReport
                {
                    Id = 5,
                    FeedbackId = 11,
                    Reason = "The reviewer expressed moderate dissatisfaction, but the wording remains within policy.",
                    Status = FeedbackReportStatusEnum.Rejected,
                    CreationDate = seedCreationDate.AddDays(11),
                    CreatedBy = 43
                },
                new FeedbackReport
                {
                    Id = 6,
                    FeedbackId = 13,
                    Reason = "This feedback mixes medicine quality and delivery experience, needs moderator decision.",
                    Status = FeedbackReportStatusEnum.Resolved,
                    CreationDate = seedCreationDate.AddDays(13),
                    CreatedBy = 44
                },
                new FeedbackReport
                {
                    Id = 7,
                    FeedbackId = 17,
                    Reason = "Negative experience was reported and checked by moderation for accuracy and wording.",
                    Status = FeedbackReportStatusEnum.Pending,
                    CreationDate = seedCreationDate.AddDays(17),
                    CreatedBy = 45
                },
                new FeedbackReport
                {
                    Id = 8,
                    FeedbackId = 20,
                    Reason = "Constructive criticism is valid and does not violate policy after review.",
                    Status = FeedbackReportStatusEnum.Rejected,
                    CreationDate = seedCreationDate.AddDays(20),
                    CreatedBy = 46
                }
            );

            return modelBuilder;
        }
    }
}
