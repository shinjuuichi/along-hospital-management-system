using MedicineSvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;

namespace MedicineSvc.DAL.Seeds
{
    public class OptionValueSeed : ISeedBuilder
    {
        public int Priority => 3;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OptionValue>().HasData(
                new OptionValue { Id = 1, OptionId = 1, ValueName = "Box", UnitMultiplier = 1, IsActive = true },
                new OptionValue { Id = 2, OptionId = 1, ValueName = "Blister", UnitMultiplier = 1, IsActive = true },
                new OptionValue { Id = 3, OptionId = 1, ValueName = "Bottle", UnitMultiplier = 1, IsActive = true },
                new OptionValue { Id = 4, OptionId = 1, ValueName = "Sachet", UnitMultiplier = 1, IsActive = true },
                new OptionValue { Id = 5, OptionId = 1, ValueName = "Tube", UnitMultiplier = 1, IsActive = true },
                new OptionValue { Id = 6, OptionId = 1, ValueName = "Packet", UnitMultiplier = 1, IsActive = true },
                new OptionValue { Id = 7, OptionId = 2, ValueName = "1 per blister", UnitMultiplier = 1, IsActive = true },
                new OptionValue { Id = 8, OptionId = 2, ValueName = "6 per blister", UnitMultiplier = 6, IsActive = true },
                new OptionValue { Id = 9, OptionId = 2, ValueName = "10 per blister", UnitMultiplier = 10, IsActive = true },
                new OptionValue { Id = 10, OptionId = 2, ValueName = "12 per blister", UnitMultiplier = 12, IsActive = true },
                new OptionValue { Id = 11, OptionId = 2, ValueName = "20 per box", UnitMultiplier = 20, IsActive = true },
                new OptionValue { Id = 12, OptionId = 2, ValueName = "30 per box", UnitMultiplier = 30, IsActive = true },
                new OptionValue { Id = 13, OptionId = 2, ValueName = "50 per box", UnitMultiplier = 50, IsActive = true },
                new OptionValue { Id = 14, OptionId = 2, ValueName = "100 per box", UnitMultiplier = 100, IsActive = true },
                new OptionValue { Id = 15, OptionId = 2, ValueName = "5ml per bottle", UnitMultiplier = 5, IsActive = true },
                new OptionValue { Id = 16, OptionId = 2, ValueName = "10ml per bottle", UnitMultiplier = 10, IsActive = true },
                new OptionValue { Id = 17, OptionId = 2, ValueName = "30ml per bottle", UnitMultiplier = 30, IsActive = true },
                new OptionValue { Id = 18, OptionId = 2, ValueName = "60ml per bottle", UnitMultiplier = 60, IsActive = true },
                new OptionValue { Id = 19, OptionId = 2, ValueName = "100ml per bottle", UnitMultiplier = 100, IsActive = true },
                new OptionValue { Id = 20, OptionId = 3, ValueName = "10mg", UnitMultiplier = 1, IsActive = true },
                new OptionValue { Id = 21, OptionId = 3, ValueName = "80mg", UnitMultiplier = 1, IsActive = true },
                new OptionValue { Id = 22, OptionId = 3, ValueName = "100mg", UnitMultiplier = 1, IsActive = true },
                new OptionValue { Id = 23, OptionId = 3, ValueName = "250mg", UnitMultiplier = 1, IsActive = true },
                new OptionValue { Id = 24, OptionId = 3, ValueName = "325mg", UnitMultiplier = 1, IsActive = true },
                new OptionValue { Id = 25, OptionId = 3, ValueName = "500mg", UnitMultiplier = 1, IsActive = true },
                new OptionValue { Id = 26, OptionId = 3, ValueName = "650mg", UnitMultiplier = 1, IsActive = true },
                new OptionValue { Id = 27, OptionId = 3, ValueName = "1000mg", UnitMultiplier = 1, IsActive = true },
                new OptionValue { Id = 28, OptionId = 4, ValueName = "5ml", UnitMultiplier = 5, IsActive = true },
                new OptionValue { Id = 29, OptionId = 4, ValueName = "10ml", UnitMultiplier = 10, IsActive = true },
                new OptionValue { Id = 30, OptionId = 4, ValueName = "30ml", UnitMultiplier = 30, IsActive = true },
                new OptionValue { Id = 31, OptionId = 4, ValueName = "60ml", UnitMultiplier = 60, IsActive = true },
                new OptionValue { Id = 32, OptionId = 4, ValueName = "100ml", UnitMultiplier = 100, IsActive = true },
                new OptionValue { Id = 33, OptionId = 4, ValueName = "250ml", UnitMultiplier = 250, IsActive = true },
                new OptionValue { Id = 34, OptionId = 4, ValueName = "500ml", UnitMultiplier = 500, IsActive = true },
                new OptionValue { Id = 35, OptionId = 5, ValueName = "5mg/ml", UnitMultiplier = 1, IsActive = true },
                new OptionValue { Id = 36, OptionId = 5, ValueName = "10mg/ml", UnitMultiplier = 1, IsActive = true },
                new OptionValue { Id = 37, OptionId = 5, ValueName = "50mg/ml", UnitMultiplier = 1, IsActive = true },
                new OptionValue { Id = 38, OptionId = 5, ValueName = "100mg/ml", UnitMultiplier = 1, IsActive = true },
                new OptionValue { Id = 39, OptionId = 5, ValueName = "5%", UnitMultiplier = 1, IsActive = true },
                new OptionValue { Id = 40, OptionId = 5, ValueName = "10%", UnitMultiplier = 1, IsActive = true }
            );

            return modelBuilder;
        }
    }
}
