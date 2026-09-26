using BillingSvc.DAL.Enums;
using BillingSvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;

namespace BillingSvc.DAL.Seeds
{
    public class RefundSeed : ISeedBuilder
    {
        public int Priority => 2;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2026, 01, 01);

            modelBuilder.Entity<Refund>().HasData(
                new Refund
                {
                    Id = 1,
                    Reason = "Failed to do service due to technical issue",
                    ChargeId = 2,
                    RefundStatus = RefundStatusEnum.Pending,
                    ClinicalMedicalOrderDetailId = "64f000000000000000000001",
                    CreationDate = seedDate,
                }
            );

            return modelBuilder;
        }
    }
}
