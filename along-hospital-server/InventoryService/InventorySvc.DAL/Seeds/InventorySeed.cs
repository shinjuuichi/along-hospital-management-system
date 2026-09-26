using InventorySvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;

namespace InventorySvc.DAL.Seeds
{
    public class InventorySeed : ISeedBuilder
    {
        public int Priority => 0;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Inventory>().HasData(

                new Inventory
                {
                    Id = 1,
                    SKUCode = "PA1BO2050",
                    Quantity = 500,
                    MinQuantity = 50,
                    MaxQuantity = 1000,
                    LastImportDate = new DateTime(2025, 10, 1, 0, 0, 0, DateTimeKind.Utc),
                    CreationDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },

                new Inventory
                {
                    Id = 2,
                    SKUCode = "PA1BO3050",
                    Quantity = 300,
                    MinQuantity = 30,
                    MaxQuantity = 800,
                    LastImportDate = new DateTime(2025, 10, 1, 0, 0, 0, DateTimeKind.Utc),
                    CreationDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },

                new Inventory
                {
                    Id = 3,
                    SKUCode = "AM2BO2050",
                    Quantity = 120,
                    MinQuantity = 20,
                    MaxQuantity = 500,
                    LastImportDate = new DateTime(2025, 10, 5, 0, 0, 0, DateTimeKind.Utc),
                    CreationDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },

                new Inventory
                {
                    Id = 4,
                    SKUCode = "AM2BL1050",
                    Quantity = 200,
                    MinQuantity = 30,
                    MaxQuantity = 500,
                    LastImportDate = new DateTime(2025, 10, 5, 0, 0, 0, DateTimeKind.Utc),
                    CreationDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },

                new Inventory
                {
                    Id = 5,
                    SKUCode = "VC3BO2010",
                    Quantity = 150,
                    MinQuantity = 20,
                    MaxQuantity = 400,
                    LastImportDate = new DateTime(2025, 10, 5, 0, 0, 0, DateTimeKind.Utc),
                    CreationDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },

                new Inventory
                {
                    Id = 6,
                    SKUCode = "PA1BL1250",
                    Quantity = 480,
                    MinQuantity = 40,
                    MaxQuantity = 900,
                    LastImportDate = new DateTime(2025, 10, 2, 0, 0, 0, DateTimeKind.Utc),
                    CreationDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },

                new Inventory
                {
                    Id = 7,
                    SKUCode = "VC3BO2050",
                    Quantity = 220,
                    MinQuantity = 25,
                    MaxQuantity = 500,
                    LastImportDate = new DateTime(2025, 10, 6, 0, 0, 0, DateTimeKind.Utc),
                    CreationDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },

                new Inventory
                {
                    Id = 8,
                    SKUCode = "AM3BO3010",
                    Quantity = 140,
                    MinQuantity = 20,
                    MaxQuantity = 350,
                    LastImportDate = new DateTime(2025, 10, 7, 0, 0, 0, DateTimeKind.Utc),
                    CreationDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },

                new Inventory
                {
                    Id = 9,
                    SKUCode = "AM3BO3260",
                    Quantity = 100,
                    MinQuantity = 15,
                    MaxQuantity = 300,
                    LastImportDate = new DateTime(2025, 10, 7, 0, 0, 0, DateTimeKind.Utc),
                    CreationDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },

                new Inventory
                {
                    Id = 10,
                    SKUCode = "BI5BO1050",
                    Quantity = 95,
                    MinQuantity = 15,
                    MaxQuantity = 250,
                    LastImportDate = new DateTime(2025, 10, 8, 0, 0, 0, DateTimeKind.Utc),
                    CreationDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },

                new Inventory
                {
                    Id = 11,
                    SKUCode = "DI6BO2050",
                    Quantity = 130,
                    MinQuantity = 20,
                    MaxQuantity = 280,
                    LastImportDate = new DateTime(2025, 10, 8, 0, 0, 0, DateTimeKind.Utc),
                    CreationDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },

                new Inventory
                {
                    Id = 12,
                    SKUCode = "NA4BO2550",
                    Quantity = 180,
                    MinQuantity = 30,
                    MaxQuantity = 420,
                    LastImportDate = new DateTime(2025, 10, 9, 0, 0, 0, DateTimeKind.Utc),
                    CreationDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },

                new Inventory
                {
                    Id = 13,
                    SKUCode = "NA7BO5010",
                    Quantity = 75,
                    MinQuantity = 10,
                    MaxQuantity = 220,
                    LastImportDate = new DateTime(2025, 10, 9, 0, 0, 0, DateTimeKind.Utc),
                    CreationDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },

                new Inventory
                {
                    Id = 14,
                    SKUCode = "OR8BO2032",
                    Quantity = 260,
                    MinQuantity = 35,
                    MaxQuantity = 520,
                    LastImportDate = new DateTime(2025, 10, 10, 0, 0, 0, DateTimeKind.Utc),
                    CreationDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );

            return modelBuilder;
        }
    }
}
