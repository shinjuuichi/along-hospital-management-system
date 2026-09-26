using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.DAL.Seeds
{
    public class HolidaySeed : ISeedBuilder
    {
        public int Priority => 0;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Holiday>().HasData(
                new Holiday
                {
                    Id = 1,
                    Day = 1,
                    Month = 1,
                    Year = null,
                    Name = "New Year's Day"
                },
                new Holiday
                {
                    Id = 2,
                    Day = 30,
                    Month = 4,
                    Year = null,
                    Name = "Reunification Day"
                },
                new Holiday
                {
                    Id = 3,
                    Day = 1,
                    Month = 5,
                    Year = null,
                    Name = "International Workers' Day"
                },
                new Holiday
                {
                    Id = 4,
                    Day = 2,
                    Month = 9,
                    Year = null,
                    Name = "National Day"
                },

                new Holiday
                {
                    Id = 5,
                    Day = 14,
                    Month = 2,
                    Year = 2026,
                    Name = "Lunar New Year's Eve (Tất Niên)"
                },
                new Holiday
                {
                    Id = 6,
                    Day = 15,
                    Month = 2,
                    Year = 2026,
                    Name = "Lunar New Year – Giao Thừa"
                },
                new Holiday
                {
                    Id = 7,
                    Day = 16,
                    Month = 2,
                    Year = 2026,
                    Name = "Lunar New Year – Mùng 1 Tết"
                },
                new Holiday
                {
                    Id = 8,
                    Day = 17,
                    Month = 2,
                    Year = 2026,
                    Name = "Lunar New Year – Mùng 2 Tết"
                },
                new Holiday
                {
                    Id = 9,
                    Day = 18,
                    Month = 2,
                    Year = 2026,
                    Name = "Lunar New Year – Mùng 3 Tết"
                },
                new Holiday
                {
                    Id = 10,
                    Day = 19,
                    Month = 2,
                    Year = 2026,
                    Name = "Lunar New Year – Mùng 4 Tết"
                },
                new Holiday
                {
                    Id = 11,
                    Day = 20,
                    Month = 2,
                    Year = 2026,
                    Name = "Lunar New Year – Mùng 5 Tết"
                }
            );

            return modelBuilder;
        }
    }
}
