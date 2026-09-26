using InpatientResourceSvc.DAL.Enums;
using InpatientResourceSvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;

namespace InpatientResourceSvc.DAL.Seeds
{
    public class BedSeed : ISeedBuilder
    {
        public int Priority => 3;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var seedCreationDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<Bed>().HasData(
                new Bed
                {
                    Id = 1,
                    Code = "A101-01",
                    Status = BedStatusEnum.Active,
                    RoomId = 1,
                    BedCategoryId = 1,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 2,
                    Code = "A101-02",
                    Status = BedStatusEnum.Occupied,
                    RoomId = 1,
                    BedCategoryId = 1,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 3,
                    Code = "A102-01",
                    Status = BedStatusEnum.Occupied,
                    RoomId = 2,
                    BedCategoryId = 2,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 4,
                    Code = "A102-02",
                    Status = BedStatusEnum.Active,
                    RoomId = 2,
                    BedCategoryId = 2,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 5,
                    Code = "B201-01",
                    Status = BedStatusEnum.Active,
                    RoomId = 3,
                    BedCategoryId = 3,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 6,
                    Code = "B201-02",
                    Status = BedStatusEnum.Maintenance,
                    RoomId = 3,
                    BedCategoryId = 3,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 7,
                    Code = "A103-01",
                    Status = BedStatusEnum.Active,
                    RoomId = 4,
                    BedCategoryId = 1,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 8,
                    Code = "A103-02",
                    Status = BedStatusEnum.Active,
                    RoomId = 4,
                    BedCategoryId = 1,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 9,
                    Code = "A104-01",
                    Status = BedStatusEnum.Active,
                    RoomId = 5,
                    BedCategoryId = 1,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 10,
                    Code = "A104-02",
                    Status = BedStatusEnum.Active,
                    RoomId = 5,
                    BedCategoryId = 1,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 11,
                    Code = "A105-01",
                    Status = BedStatusEnum.Active,
                    RoomId = 6,
                    BedCategoryId = 1,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 12,
                    Code = "A105-02",
                    Status = BedStatusEnum.Maintenance,
                    RoomId = 6,
                    BedCategoryId = 1,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 13,
                    Code = "A106-01",
                    Status = BedStatusEnum.Active,
                    RoomId = 7,
                    BedCategoryId = 1,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 14,
                    Code = "A106-02",
                    Status = BedStatusEnum.Active,
                    RoomId = 7,
                    BedCategoryId = 1,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 15,
                    Code = "A107-01",
                    Status = BedStatusEnum.Active,
                    RoomId = 8,
                    BedCategoryId = 2,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 16,
                    Code = "A107-02",
                    Status = BedStatusEnum.Active,
                    RoomId = 8,
                    BedCategoryId = 2,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 17,
                    Code = "A108-01",
                    Status = BedStatusEnum.Active,
                    RoomId = 9,
                    BedCategoryId = 1,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 18,
                    Code = "A108-02",
                    Status = BedStatusEnum.Active,
                    RoomId = 9,
                    BedCategoryId = 1,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 19,
                    Code = "B202-01",
                    Status = BedStatusEnum.Active,
                    RoomId = 10,
                    BedCategoryId = 1,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 20,
                    Code = "B202-02",
                    Status = BedStatusEnum.Active,
                    RoomId = 10,
                    BedCategoryId = 1,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 21,
                    Code = "B203-01",
                    Status = BedStatusEnum.Active,
                    RoomId = 11,
                    BedCategoryId = 1,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 22,
                    Code = "B203-02",
                    Status = BedStatusEnum.Active,
                    RoomId = 11,
                    BedCategoryId = 1,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 23,
                    Code = "B204-01",
                    Status = BedStatusEnum.Active,
                    RoomId = 12,
                    BedCategoryId = 1,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 24,
                    Code = "B204-02",
                    Status = BedStatusEnum.Active,
                    RoomId = 12,
                    BedCategoryId = 1,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 25,
                    Code = "B205-01",
                    Status = BedStatusEnum.Active,
                    RoomId = 13,
                    BedCategoryId = 1,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 26,
                    Code = "B205-02",
                    Status = BedStatusEnum.Maintenance,
                    RoomId = 13,
                    BedCategoryId = 1,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 27,
                    Code = "B206-01",
                    Status = BedStatusEnum.Active,
                    RoomId = 14,
                    BedCategoryId = 1,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 28,
                    Code = "B206-02",
                    Status = BedStatusEnum.Active,
                    RoomId = 14,
                    BedCategoryId = 1,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 29,
                    Code = "B207-01",
                    Status = BedStatusEnum.Active,
                    RoomId = 15,
                    BedCategoryId = 1,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 30,
                    Code = "B207-02",
                    Status = BedStatusEnum.Active,
                    RoomId = 15,
                    BedCategoryId = 1,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 31,
                    Code = "B208-01",
                    Status = BedStatusEnum.Active,
                    RoomId = 16,
                    BedCategoryId = 1,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 32,
                    Code = "B208-02",
                    Status = BedStatusEnum.Active,
                    RoomId = 16,
                    BedCategoryId = 1,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 33,
                    Code = "B209-01",
                    Status = BedStatusEnum.Active,
                    RoomId = 17,
                    BedCategoryId = 2,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 34,
                    Code = "B209-02",
                    Status = BedStatusEnum.Active,
                    RoomId = 17,
                    BedCategoryId = 2,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 35,
                    Code = "B210-01",
                    Status = BedStatusEnum.Active,
                    RoomId = 18,
                    BedCategoryId = 1,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 36,
                    Code = "B210-02",
                    Status = BedStatusEnum.Active,
                    RoomId = 18,
                    BedCategoryId = 1,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 37,
                    Code = "C101-01",
                    Status = BedStatusEnum.Active,
                    RoomId = 19,
                    BedCategoryId = 1,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 38,
                    Code = "C101-02",
                    Status = BedStatusEnum.Active,
                    RoomId = 19,
                    BedCategoryId = 1,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 39,
                    Code = "C102-01",
                    Status = BedStatusEnum.Active,
                    RoomId = 20,
                    BedCategoryId = 1,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 40,
                    Code = "C102-02",
                    Status = BedStatusEnum.Active,
                    RoomId = 20,
                    BedCategoryId = 1,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 41,
                    Code = "C103-01",
                    Status = BedStatusEnum.Active,
                    RoomId = 21,
                    BedCategoryId = 3,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 42,
                    Code = "C103-02",
                    Status = BedStatusEnum.Active,
                    RoomId = 21,
                    BedCategoryId = 3,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 43,
                    Code = "C104-01",
                    Status = BedStatusEnum.Active,
                    RoomId = 22,
                    BedCategoryId = 3,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 44,
                    Code = "C104-02",
                    Status = BedStatusEnum.Active,
                    RoomId = 22,
                    BedCategoryId = 3,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 45,
                    Code = "C105-01",
                    Status = BedStatusEnum.Active,
                    RoomId = 23,
                    BedCategoryId = 1,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 46,
                    Code = "C105-02",
                    Status = BedStatusEnum.Active,
                    RoomId = 23,
                    BedCategoryId = 1,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 47,
                    Code = "C106-01",
                    Status = BedStatusEnum.Active,
                    RoomId = 24,
                    BedCategoryId = 1,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 48,
                    Code = "C106-02",
                    Status = BedStatusEnum.Active,
                    RoomId = 24,
                    BedCategoryId = 1,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 49,
                    Code = "C107-01",
                    Status = BedStatusEnum.Active,
                    RoomId = 25,
                    BedCategoryId = 1,
                    CreationDate = seedCreationDate
                },
                new Bed
                {
                    Id = 50,
                    Code = "C107-02",
                    Status = BedStatusEnum.Active,
                    RoomId = 25,
                    BedCategoryId = 1,
                    CreationDate = seedCreationDate
                }
            );

            return modelBuilder;
        }
    }
}
