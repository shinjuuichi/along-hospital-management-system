using BillingSvc.DAL.Enums;
using BillingSvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;

namespace BillingSvc.DAL.Seeds
{
    public class ChargeSeed : ISeedBuilder
    {
        public int Priority => 1;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2026, 01, 01);

            modelBuilder.Entity<Charge>().HasData(
                new Charge
                {
                    Id = 1,
                    InvoiceId = 1,
                    MedicalServiceId = 1,
                    Quantity = 1,
                    UnitPrice = 500,
                    CreationDate = seedDate,
                    ChargeSnapshot = new ChargeSnapshot
                    {
                        MedicalServiceName = "General Health Check",
                        MedicalServiceDescription = "Comprehensive periodic health check-up"
                    }
                },
                new Charge
                {
                    Id = 2,
                    InvoiceId = 1,
                    MedicalServiceId = 2,
                    ChargeType = ChargeTypeEnum.Refund,
                    Quantity = 1,
                    UnitPrice = 700,
                    CreationDate = seedDate,
                    ChargeSnapshot = new ChargeSnapshot
                    {
                        MedicalServiceName = "Cardiology Consultation",
                        MedicalServiceDescription = "Heart and vascular health consultation"
                    }
                },
                new Charge
                {
                    Id = 3,
                    InvoiceId = 2,
                    MedicalServiceId = 3,
                    Quantity = 1,
                    UnitPrice = 600,
                    CreationDate = seedDate.AddDays(2),
                    ChargeSnapshot = new ChargeSnapshot
                    {
                        MedicalServiceName = "Dermatology Consultation",
                        MedicalServiceDescription = "Skin health and treatment consultation"
                    }
                },
                new Charge
                {
                    Id = 4,
                    InvoiceId = 3,
                    MedicalServiceId = 1,
                    Quantity = 2,
                    UnitPrice = 500,
                    CreationDate = seedDate.AddDays(3),
                    ChargeSnapshot = new ChargeSnapshot
                    {
                        MedicalServiceName = "General Health Check",
                        MedicalServiceDescription = "Comprehensive periodic health check-up"
                    }
                }
            );

            return modelBuilder;
        }
    }
}