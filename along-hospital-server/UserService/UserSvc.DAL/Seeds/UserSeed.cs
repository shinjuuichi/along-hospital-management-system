using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;
using SharedLibrary.Enums;
using UserSvc.DAL.Enums;
using UserSvc.DAL.Models;

namespace UserSvc.DAL.Seeds
{
    public class UserSeed : ISeedBuilder
    {
        public int Priority => 1;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var seedCreationDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc);
            var doctorNames = new[]
            {
                "Nguyen Quoc Bao",
                "Tran Minh Quan",
                "Le Hoang Phuc",
                "Pham Duc Thinh",
                "Vo Gia Huy",
                "Dang Tuan Kiet",
                "Bui Thanh Nam",
                "Doan Quang Hiep",
                "Hoang Anh Khoa"
            };
            var doctorImages = new[]
            {
                "Staff/e775bca93b33420da05be0ff2260bf55.webp",
                "Staff/93f6e4c206bf4dd68bba2451c45f6f10.webp",
                "Staff/511d363b7db842c19a5f302a208b65e2.webp",
                "Staff/4aac037d513a4a3cb8e46f51bbd5a9f5.webp",
                "Staff/700ecf6913ab4417959e6bfababf22e0.webp",
                "Staff/a7b1670d959f43059676e25b2a07eabf.webp",
                "Staff/9ef3ae9834be48ab93407983fcb17711.webp",
                "Staff/3af8d6407b7446638ab1d3f015f49290.webp",
                "Staff/9168d20b24264cd88731852aa898501f.webp"
            };
            var patientNames = new[]
            {
                "Nguyen Hoang Minh",
                "Tran Gia Han",
                "Le Duc Anh",
                "Pham Thu Trang",
                "Vo Quynh Nhu",
                "Dang Thanh Tung",
                "Bui Ngoc Mai",
                "Do Gia Bao",
                "Hoang Tu Linh",
                "Phan Minh Khang",
                "Huynh Bao Chau",
                "Ngo Thanh Truc",
                "Truong Gia Huy",
                "Duong Nhat Vy",
                "Ly Minh Thu",
                "Mai Quoc Khanh",
                "Cao Bao Ngoc",
                "Dinh Thanh Ha",
                "Chu Ngoc Han",
                "Lam Minh Quan",
                "Ta Bao Tran",
                "Ton That Nhu Y",
                "Kieu Thanh Nhan",
                "Quach Bao Han",
                "Thai Anh Thu"
            };

            List<User> users =
            [
                new User
                {
                    Id = 2,
                    Name = "Manager User",
                    Role = RoleEnum.Manager,
                    DateOfBirth = new DateOnly(1983, 3, 15),
                    Gender = GenderEnum.Male,
                    Address = "111 Dong Khoi, quan 1, thanh pho HCM",
                    Image = null,
                    CreationDate = seedCreationDate,
                },
                new User
                {
                    Id = 3,
                    Name = "Patient User",
                    Role = RoleEnum.Patient,
                    DateOfBirth = new DateOnly(1992, 8, 15),
                    Gender = GenderEnum.Male,
                    Address = "222 Ly Thuong Kiet, quan 10, thanh pho HCM",
                    Image = null,
                    CreationDate = seedCreationDate,
                },
                new User
                {
                    Id = 4,
                    Name = "Doctor User",
                    Role = RoleEnum.Doctor,
                    DateOfBirth = new DateOnly(1980, 3, 10),
                    Gender = GenderEnum.Male,
                    Address = "333 Nguyen Van Cu, quan 5, thanh pho HCM",
                    Image = "Staff/2799d78a52ba482aabb2fff1552a11b4.webp",
                    CreationDate = seedCreationDate,
                },
                new User
                {
                    Id = 5,
                    Name = "Nurse User",
                    Role = RoleEnum.Nurse,
                    DateOfBirth = new DateOnly(1990, 5, 20),
                    Gender = GenderEnum.Female,
                    Address = "444 An Duong Vuong, quan 5, thanh pho HCM",
                    Image = null,
                    CreationDate = seedCreationDate,
                },
                new User
                {
                    Id = 6,
                    Name = "HR User",
                    Role = RoleEnum.HR,
                    DateOfBirth = new DateOnly(1988, 11, 25),
                    Gender = GenderEnum.Female,
                    Address = "555 Le Van Sy, quan 3, thanh pho HCM",
                    Image = null,
                    CreationDate = seedCreationDate,
                },
                new User
                {
                    Id = 7,
                    Name = "Pharmacist User",
                    Role = RoleEnum.Pharmacist,
                    DateOfBirth = new DateOnly(1987, 7, 12),
                    Gender = GenderEnum.Male,
                    Address = "666 Cach Mang Thang 8, quan 10, thanh pho HCM",
                    Image = null,
                    CreationDate = seedCreationDate,
                },
                new User
                {
                    Id = 8,
                    Name = "Accountant User",
                    Role = RoleEnum.Accountant,
                    DateOfBirth = new DateOnly(1991, 4, 18),
                    Gender = GenderEnum.Female,
                    Address = "777 Nguyen Thi Minh Khai, quan 1, thanh pho HCM",
                    Image = null,
                    CreationDate = seedCreationDate,
                },
                new User
                {
                    Id = 9,
                    Name = "Marketer User",
                    Role = RoleEnum.Marketer,
                    DateOfBirth = new DateOnly(1993, 9, 5),
                    Gender = GenderEnum.Male,
                    Address = "888 Dien Bien Phu, quan Binh Thanh, thanh pho HCM",
                    Image = null,
                    CreationDate = seedCreationDate,
                },
                new User
                {
                    Id = 11,
                    Name = "Receptionist User",
                    Role = RoleEnum.Receptionist,
                    DateOfBirth = new DateOnly(1995, 6, 14),
                    Gender = GenderEnum.Female,
                    Address = "100 Hoang Van Thu, quan Tan Binh, thanh pho HCM",
                    Image = null,
                    CreationDate = seedCreationDate,
                },
                new User
                {
                    Id = 12,
                    Name = "Hotline Agent User",
                    Role = RoleEnum.HotlineAgent,
                    DateOfBirth = new DateOnly(1994, 10, 22),
                    Gender = GenderEnum.Male,
                    Address = "200 Truong Chinh, quan Tan Phu, thanh pho HCM",
                    Image = null,
                    CreationDate = seedCreationDate,
                },
                new User
                {
                    Id = 13,
                    Name = "Inventory Clerk User",
                    Role = RoleEnum.InventoryClerk,
                    DateOfBirth = new DateOnly(1996, 12, 8),
                    Gender = GenderEnum.Male,
                    Address = "300 Luy Ban Bich, quan Tan Phu, thanh pho HCM",
                    Image = null,
                    CreationDate = seedCreationDate,
                }
            ];

            for (var specialtyId = 1; specialtyId <= 9; specialtyId++)
            {
                var specialtyIndex = specialtyId - 1;
                var doctorId = 13 + specialtyId;

                users.Add(new User
                {
                    Id = doctorId,
                    Name = doctorNames[specialtyIndex],
                    Role = RoleEnum.Doctor,
                    DateOfBirth = new DateOnly(1978 + (specialtyIndex % 12), (specialtyIndex % 12) + 1, (specialtyIndex % 27) + 1),
                    Gender = specialtyIndex % 2 == 0 ? GenderEnum.Male : GenderEnum.Female,
                    Address = $"{150 + specialtyIndex} Nguyen Du, quan {(specialtyIndex % 6) + 1}, thanh pho HCM",
                    Image = doctorImages[specialtyIndex],
                    CreationDate = seedCreationDate
                });
            }

            for (var patientIndex = 0; patientIndex < 25; patientIndex++)
            {
                var patientId = 39 + patientIndex;

                users.Add(new User
                {
                    Id = patientId,
                    Name = patientNames[patientIndex],
                    Role = RoleEnum.Patient,
                    DateOfBirth = new DateOnly(1989 + (patientIndex % 12), ((patientIndex + 3) % 12) + 1, ((patientIndex * 2) % 27) + 1),
                    Gender = patientIndex % 2 == 0 ? GenderEnum.Male : GenderEnum.Female,
                    Address = $"{210 + patientIndex} Dien Bien Phu, quan Binh Thanh, thanh pho HCM",
                    Image = null,
                    CreationDate = seedCreationDate
                });
            }

            modelBuilder.Entity<User>().HasData(users.ToArray());

            return modelBuilder;
        }
    }
}
