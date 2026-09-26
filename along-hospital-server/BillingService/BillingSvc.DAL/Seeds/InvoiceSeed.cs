using BillingSvc.DAL.Enums;
using BillingSvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;

namespace BillingSvc.DAL.Seeds
{
    public class InvoiceSeed : ISeedBuilder
    {
        public int Priority => 0;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2026, 01, 01);

            modelBuilder.Entity<Invoice>().HasData(
                new Invoice
                {
                    Id = 1,
                    InvoiceNumber = $"INV-OPD-{seedDate:yyyyMMdd}-{seedDate.Ticks % 1000000:D6}",
                    InvoiceStatus = InvoiceStatusEnum.Completed,
                    MedicalHistoryId = 1,
                    ClinicalMedicalOrderId = "64f000000000000000000001",
                    CreationDate = seedDate,
                    PaymentDate = seedDate.AddDays(1),
                },
                new Invoice
                {
                    Id = 2,
                    InvoiceNumber = $"INV-OPD-{seedDate.AddDays(1):yyyyMMdd}-{(seedDate.AddDays(1).Ticks) % 1000000:D6}",
                    InvoiceStatus = InvoiceStatusEnum.Completed,
                    MedicalHistoryId = 1,
                    CreationDate = seedDate.AddDays(2),
                    PaymentDate = seedDate.AddDays(3),
                },
                new Invoice
                {
                    Id = 3,
                    InvoiceNumber = $"INV-IPD-{seedDate.AddDays(2):yyyyMMdd}-{(seedDate.AddDays(2).Ticks) % 1000000:D6}",
                    InvoiceStatus = InvoiceStatusEnum.Completed,
                    MedicalHistoryId = 2,
                    ClinicalMedicalOrderId = "64f000000000000000000003",
                    CreationDate = seedDate.AddDays(3),
                    PaymentDate = seedDate.AddDays(4),
                }
            );

            return modelBuilder;
        }
    }
}