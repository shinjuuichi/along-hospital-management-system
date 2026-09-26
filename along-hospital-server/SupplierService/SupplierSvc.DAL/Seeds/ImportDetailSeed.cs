using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;
using SupplierSvc.DAL.Models;

namespace SupplierSvc.DAL.Seeds
{
    public class ImportDetailSeed : ISeedBuilder
    {
        public int Priority => 3;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ImportDetail>().HasData(
                new ImportDetail
                {
                    ImportId = 1,
                    SKUCode = "PA1BO2050",
                    Quantity = 100,
                    UnitPrice = 100,
                },
                new ImportDetail
                {
                    ImportId = 2,
                    SKUCode = "AM2BO2050",
                    Quantity = 200,
                    UnitPrice = 100,
                },
                new ImportDetail
                {
                    ImportId = 3,
                    SKUCode = "VC3BO2010",
                    Quantity = 300,
                    UnitPrice = 100,
                }
            );

            return modelBuilder;
        }
    }
}