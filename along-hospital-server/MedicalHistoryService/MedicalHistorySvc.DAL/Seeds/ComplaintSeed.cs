using MedicalHistorySvc.DAL.Enums;
using MedicalHistorySvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;

namespace MedicalHistorySvc.DAL.Seeds
{
    public class ComplaintSeed : ISeedBuilder
    {
        public int Priority => 0;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2026, 1, 1);

            modelBuilder.Entity<Complaint>().HasData(
                new Complaint
                {
                    Id = 1,
                    ComplaintTopic = ComplaintTopicEnum.Service,
                    Content = "The waiting time was too long.",
                    Response = "We are sorry for the delay. We'll improve scheduling.",
                    ComplaintType = ComplaintTypeEnum.Negative,
                    ComplaintResolveStatus = ComplaintResolveStatusEnum.Resolved,
                    MedicalHistoryId = 1,
                    CreationDate = seedDate,
                },
                new Complaint
                {
                    Id = 2,
                    ComplaintTopic = ComplaintTopicEnum.Doctor,
                    Content = "Doctor was very attentive and professional.",
                    Response = "Thank you for your feedback.",
                    ComplaintType = ComplaintTypeEnum.Positive,
                    ComplaintResolveStatus = ComplaintResolveStatusEnum.Closed,
                    MedicalHistoryId = 2,
                    CreationDate = seedDate.AddDays(2),
                }
            );

            modelBuilder.Entity<ComplaintSummary>().HasData(
                new ComplaintSummary
                {
                    Id = 1,
                    Year = 2026,
                    WeekOfYear = 1,
                    Summary = "In the first week of 2026, we received 10 complaints: 6 negative, 3 neutral, and 1 positive.",
                });

            return modelBuilder;
        }
    }
}
