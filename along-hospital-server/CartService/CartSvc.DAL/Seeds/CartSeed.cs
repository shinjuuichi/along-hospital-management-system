using CartSvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;

namespace CartSvc.DAL.Seeds
{
    public class CartSeed : ISeedBuilder
    {
        public int Priority => 0;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            List<Cart> carts =
            [
                new()
                {
                    Id = 1,
                    PatientId = 3
                },
            ];

            for (var patientIndex = 0; patientIndex < 25; patientIndex++)
            {
                carts.Add(new Cart
                {
                    Id = patientIndex + 2,
                    PatientId = 39 + patientIndex
                });
            }

            modelBuilder.Entity<Cart>().HasData(carts.ToArray());

            return modelBuilder;
        }
    }
}
