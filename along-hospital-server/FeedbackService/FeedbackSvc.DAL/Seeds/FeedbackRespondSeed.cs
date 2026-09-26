using FeedbackSvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;

namespace FeedbackSvc.DAL.Seeds
{
    public class FeedbackRespondSeed : ISeedBuilder
    {
        public int Priority => 2;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var seedCreationDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<FeedbackRespond>().HasData(
                new FeedbackRespond
                {
                    Id = 1,
                    Content = "Thank you for your positive feedback! We're glad it helped.",
                    FeedbackId = 1,
                    CreationDate = seedCreationDate,
                    CreatedBy = 12
                },
                new FeedbackRespond
                {
                    Id = 2,
                    Content = "We appreciate your honesty. Please consult your doctor if dizziness continues.",
                    FeedbackId = 3,
                    CreationDate = seedCreationDate.AddDays(2),
                    CreatedBy = 12
                },
                new FeedbackRespond
                {
                    Id = 3,
                    Content = "Thank you for sharing. We are happy the medicine reduced your fever quickly.",
                    FeedbackId = 4,
                    CreationDate = seedCreationDate.AddDays(3),
                    CreatedBy = 11
                },
                new FeedbackRespond
                {
                    Id = 4,
                    Content = "We appreciate your review and are glad the eye drops felt comfortable to use.",
                    FeedbackId = 6,
                    CreationDate = seedCreationDate.AddDays(5),
                    CreatedBy = 12
                },
                new FeedbackRespond
                {
                    Id = 5,
                    Content = "Thank you for your feedback. We are glad the medicine supported your recovery.",
                    FeedbackId = 8,
                    CreationDate = seedCreationDate.AddDays(7),
                    CreatedBy = 11
                },
                new FeedbackRespond
                {
                    Id = 6,
                    Content = "We appreciate the review. Our team is happy to know the eye drops were gentle for you.",
                    FeedbackId = 10,
                    CreationDate = seedCreationDate.AddDays(9),
                    CreatedBy = 12
                },
                new FeedbackRespond
                {
                    Id = 7,
                    Content = "Thank you for keeping this product at home and using it as directed.",
                    FeedbackId = 12,
                    CreationDate = seedCreationDate.AddDays(11),
                    CreatedBy = 11
                },
                new FeedbackRespond
                {
                    Id = 8,
                    Content = "We are glad the vitamin tablets were easy to include in your daily routine.",
                    FeedbackId = 14,
                    CreationDate = seedCreationDate.AddDays(13),
                    CreatedBy = 12
                },
                new FeedbackRespond
                {
                    Id = 9,
                    Content = "Thank you for your feedback. We are pleased the powder was convenient during illness.",
                    FeedbackId = 16,
                    CreationDate = seedCreationDate.AddDays(15),
                    CreatedBy = 11
                },
                new FeedbackRespond
                {
                    Id = 10,
                    Content = "We appreciate your detailed review and are glad the pain relief was reliable.",
                    FeedbackId = 18,
                    CreationDate = seedCreationDate.AddDays(17),
                    CreatedBy = 12
                },
                new FeedbackRespond
                {
                    Id = 11,
                    Content = "Thank you for the suggestion. We will review the product instructions for clarity.",
                    FeedbackId = 20,
                    CreationDate = seedCreationDate.AddDays(19),
                    CreatedBy = 11
                }
            );

            return modelBuilder;
        }
    }
}
