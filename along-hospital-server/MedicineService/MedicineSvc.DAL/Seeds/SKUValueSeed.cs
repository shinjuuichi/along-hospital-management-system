using MedicineSvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;

namespace MedicineSvc.DAL.Seeds
{
    public class SKUValueSeed : ISeedBuilder
    {
        public int Priority => 7;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SKUValue>().HasData(
                new SKUValue { MedicineSKUId = 1, OptionValueId = 1 },
                new SKUValue { MedicineSKUId = 1, OptionValueId = 11 },
                new SKUValue { MedicineSKUId = 1, OptionValueId = 25 },

                new SKUValue { MedicineSKUId = 2, OptionValueId = 1 },
                new SKUValue { MedicineSKUId = 2, OptionValueId = 12 },
                new SKUValue { MedicineSKUId = 2, OptionValueId = 25 },

                new SKUValue { MedicineSKUId = 3, OptionValueId = 2 },
                new SKUValue { MedicineSKUId = 3, OptionValueId = 10 },
                new SKUValue { MedicineSKUId = 3, OptionValueId = 25 },

                new SKUValue { MedicineSKUId = 4, OptionValueId = 1 },
                new SKUValue { MedicineSKUId = 4, OptionValueId = 11 },
                new SKUValue { MedicineSKUId = 4, OptionValueId = 25 },

                new SKUValue { MedicineSKUId = 5, OptionValueId = 2 },
                new SKUValue { MedicineSKUId = 5, OptionValueId = 9 },
                new SKUValue { MedicineSKUId = 5, OptionValueId = 25 },

                new SKUValue { MedicineSKUId = 6, OptionValueId = 1 },
                new SKUValue { MedicineSKUId = 6, OptionValueId = 11 },
                new SKUValue { MedicineSKUId = 6, OptionValueId = 27 },

                new SKUValue { MedicineSKUId = 7, OptionValueId = 1 },
                new SKUValue { MedicineSKUId = 7, OptionValueId = 12 },
                new SKUValue { MedicineSKUId = 7, OptionValueId = 27 },

                new SKUValue { MedicineSKUId = 8, OptionValueId = 1 },
                new SKUValue { MedicineSKUId = 8, OptionValueId = 17 },
                new SKUValue { MedicineSKUId = 8, OptionValueId = 36 },

                new SKUValue { MedicineSKUId = 9, OptionValueId = 1 },
                new SKUValue { MedicineSKUId = 9, OptionValueId = 18 },
                new SKUValue { MedicineSKUId = 9, OptionValueId = 39 },

                new SKUValue { MedicineSKUId = 10, OptionValueId = 1 },
                new SKUValue { MedicineSKUId = 10, OptionValueId = 29 },
                new SKUValue { MedicineSKUId = 10, OptionValueId = 39 },

                new SKUValue { MedicineSKUId = 11, OptionValueId = 1 },
                new SKUValue { MedicineSKUId = 11, OptionValueId = 11 },
                new SKUValue { MedicineSKUId = 11, OptionValueId = 39 },

                new SKUValue { MedicineSKUId = 12, OptionValueId = 1 },
                new SKUValue { MedicineSKUId = 12, OptionValueId = 33 },
                new SKUValue { MedicineSKUId = 12, OptionValueId = 37 },

                new SKUValue { MedicineSKUId = 13, OptionValueId = 1 },
                new SKUValue { MedicineSKUId = 13, OptionValueId = 34 },
                new SKUValue { MedicineSKUId = 13, OptionValueId = 38 },

                new SKUValue { MedicineSKUId = 14, OptionValueId = 1 },
                new SKUValue { MedicineSKUId = 14, OptionValueId = 11 },
                new SKUValue { MedicineSKUId = 14, OptionValueId = 24 }
            );

            return modelBuilder;
        }
    }
}
