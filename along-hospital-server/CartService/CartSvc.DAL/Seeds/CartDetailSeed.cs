using CartSvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;

namespace CartSvc.DAL.Seeds
{
    public class CartDetailSeed : ISeedBuilder
    {
        public int Priority => 1;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CartDetail>().HasData(
                new
                {
                    CartId = 1,
                    SKUCode = "PA1BO2050",
                    Quantity = 2
                },
                new
                {
                    CartId = 1,
                    SKUCode = "PA1BO3050",
                    Quantity = 1
                },
                new
                {
                    CartId = 1,
                    SKUCode = "AM2BO2050",
                    Quantity = 3
                },
                new
                {
                    CartId = 2,
                    SKUCode = "AM2BL1050",
                    Quantity = 1
                },
                new
                {
                    CartId = 2,
                    SKUCode = "VC3BO2010",
                    Quantity = 4
                },
                new
                {
                    CartId = 2,
                    SKUCode = "PA1BO2050",
                    Quantity = 2
                }
            );

            return modelBuilder;
        }
    }

}
