using InpatientResourceSvc.DAL.Enums;
using InpatientResourceSvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;

namespace InpatientResourceSvc.DAL.Seeds
{
    public class RoomSeed : ISeedBuilder
    {
        public int Priority => 2;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var seedCreationDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<Room>().HasData(
                new Room
                {
                    Id = 1,
                    Code = "A101",
                    Status = RoomStatusEnum.Active,
                    FloorId = 1,
                    RoomCategoryId = 1,
                    SpecialtyId = 1,
                    CreationDate = seedCreationDate
                },
                new Room
                {
                    Id = 2,
                    Code = "A102",
                    Status = RoomStatusEnum.Active,
                    FloorId = 1,
                    RoomCategoryId = 2,
                    SpecialtyId = 2,
                    CreationDate = seedCreationDate
                },
                new Room
                {
                    Id = 3,
                    Code = "B201",
                    Status = RoomStatusEnum.Active,
                    FloorId = 2,
                    RoomCategoryId = 3,
                    SpecialtyId = 3,
                    CreationDate = seedCreationDate
                },
                new Room
                {
                    Id = 4,
                    Code = "A103",
                    Status = RoomStatusEnum.Active,
                    FloorId = 1,
                    RoomCategoryId = 1,
                    SpecialtyId = 4,
                    CreationDate = seedCreationDate
                },
                new Room
                {
                    Id = 5,
                    Code = "A104",
                    Status = RoomStatusEnum.Active,
                    FloorId = 1,
                    RoomCategoryId = 1,
                    SpecialtyId = 5,
                    CreationDate = seedCreationDate
                },
                new Room
                {
                    Id = 6,
                    Code = "A105",
                    Status = RoomStatusEnum.Active,
                    FloorId = 1,
                    RoomCategoryId = 1,
                    SpecialtyId = 6,
                    CreationDate = seedCreationDate
                },
                new Room
                {
                    Id = 7,
                    Code = "A106",
                    Status = RoomStatusEnum.Active,
                    FloorId = 1,
                    RoomCategoryId = 1,
                    SpecialtyId = 7,
                    CreationDate = seedCreationDate
                },
                new Room
                {
                    Id = 8,
                    Code = "A107",
                    Status = RoomStatusEnum.Active,
                    FloorId = 1,
                    RoomCategoryId = 2,
                    SpecialtyId = 8,
                    CreationDate = seedCreationDate
                },
                new Room
                {
                    Id = 9,
                    Code = "A108",
                    Status = RoomStatusEnum.Active,
                    FloorId = 1,
                    RoomCategoryId = 1,
                    SpecialtyId = 9,
                    CreationDate = seedCreationDate
                },
                new Room
                {
                    Id = 10,
                    Code = "B202",
                    Status = RoomStatusEnum.Active,
                    FloorId = 2,
                    RoomCategoryId = 1,
                    SpecialtyId = 10,
                    CreationDate = seedCreationDate
                },
                new Room
                {
                    Id = 11,
                    Code = "B203",
                    Status = RoomStatusEnum.Active,
                    FloorId = 2,
                    RoomCategoryId = 1,
                    SpecialtyId = 11,
                    CreationDate = seedCreationDate
                },
                new Room
                {
                    Id = 12,
                    Code = "B204",
                    Status = RoomStatusEnum.Active,
                    FloorId = 2,
                    RoomCategoryId = 1,
                    SpecialtyId = 12,
                    CreationDate = seedCreationDate
                },
                new Room
                {
                    Id = 13,
                    Code = "B205",
                    Status = RoomStatusEnum.Active,
                    FloorId = 2,
                    RoomCategoryId = 1,
                    SpecialtyId = 13,
                    CreationDate = seedCreationDate
                },
                new Room
                {
                    Id = 14,
                    Code = "B206",
                    Status = RoomStatusEnum.Active,
                    FloorId = 2,
                    RoomCategoryId = 1,
                    SpecialtyId = 14,
                    CreationDate = seedCreationDate
                },
                new Room
                {
                    Id = 15,
                    Code = "B207",
                    Status = RoomStatusEnum.Active,
                    FloorId = 2,
                    RoomCategoryId = 1,
                    SpecialtyId = 15,
                    CreationDate = seedCreationDate
                },
                new Room
                {
                    Id = 16,
                    Code = "B208",
                    Status = RoomStatusEnum.Active,
                    FloorId = 2,
                    RoomCategoryId = 1,
                    SpecialtyId = 16,
                    CreationDate = seedCreationDate
                },
                new Room
                {
                    Id = 17,
                    Code = "B209",
                    Status = RoomStatusEnum.Active,
                    FloorId = 2,
                    RoomCategoryId = 2,
                    SpecialtyId = 17,
                    CreationDate = seedCreationDate
                },
                new Room
                {
                    Id = 18,
                    Code = "B210",
                    Status = RoomStatusEnum.Active,
                    FloorId = 2,
                    RoomCategoryId = 1,
                    SpecialtyId = 18,
                    CreationDate = seedCreationDate
                },
                new Room
                {
                    Id = 19,
                    Code = "C101",
                    Status = RoomStatusEnum.Active,
                    FloorId = 3,
                    RoomCategoryId = 1,
                    SpecialtyId = 19,
                    CreationDate = seedCreationDate
                },
                new Room
                {
                    Id = 20,
                    Code = "C102",
                    Status = RoomStatusEnum.Active,
                    FloorId = 3,
                    RoomCategoryId = 1,
                    SpecialtyId = 20,
                    CreationDate = seedCreationDate
                },
                new Room
                {
                    Id = 21,
                    Code = "C103",
                    Status = RoomStatusEnum.Active,
                    FloorId = 3,
                    RoomCategoryId = 3,
                    SpecialtyId = 21,
                    CreationDate = seedCreationDate
                },
                new Room
                {
                    Id = 22,
                    Code = "C104",
                    Status = RoomStatusEnum.Active,
                    FloorId = 3,
                    RoomCategoryId = 3,
                    SpecialtyId = 22,
                    CreationDate = seedCreationDate
                },
                new Room
                {
                    Id = 23,
                    Code = "C105",
                    Status = RoomStatusEnum.Active,
                    FloorId = 3,
                    RoomCategoryId = 1,
                    SpecialtyId = 23,
                    CreationDate = seedCreationDate
                },
                new Room
                {
                    Id = 24,
                    Code = "C106",
                    Status = RoomStatusEnum.Active,
                    FloorId = 3,
                    RoomCategoryId = 1,
                    SpecialtyId = 24,
                    CreationDate = seedCreationDate
                },
                new Room
                {
                    Id = 25,
                    Code = "C107",
                    Status = RoomStatusEnum.Active,
                    FloorId = 3,
                    RoomCategoryId = 1,
                    SpecialtyId = 25,
                    CreationDate = seedCreationDate
                }
            );

            return modelBuilder;
        }
    }
}
