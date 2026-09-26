using Microsoft.EntityFrameworkCore;
using PayrollSvc.DAL.Models;
using SharedLibrary.Base.Data;

namespace PayrollSvc.DAL.Seeds
{
    public class TaxBracketSeed : ISeedBuilder
    {
        public int Priority => 0;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<TaxBracket>().HasData(
                new TaxBracket { Id = 1, FromAmount = 0, TaxRate = 0.05, CreationDate = seedDate },
                new TaxBracket { Id = 2, FromAmount = 200, TaxRate = 0.10, CreationDate = seedDate },
                new TaxBracket { Id = 3, FromAmount = 400, TaxRate = 0.15, CreationDate = seedDate },
                new TaxBracket { Id = 4, FromAmount = 720, TaxRate = 0.20, CreationDate = seedDate },
                new TaxBracket { Id = 5, FromAmount = 1280, TaxRate = 0.25, CreationDate = seedDate },
                new TaxBracket { Id = 6, FromAmount = 2080, TaxRate = 0.30, CreationDate = seedDate },
                new TaxBracket { Id = 7, FromAmount = 3200, TaxRate = 0.35, CreationDate = seedDate }
            );

            return modelBuilder;
        }
    }
}
