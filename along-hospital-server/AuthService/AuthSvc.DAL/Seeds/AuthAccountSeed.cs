using AuthSvc.DAL.Enums;
using AuthSvc.DAL.Models;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;

namespace AuthSvc.DAL.Seeds
{
    public class AuthAccountSeed : ISeedBuilder
    {
        public int Priority => 12;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            const string defaultPasswordHash = "$2y$12$ircZ0HzeGiPj9YCFwyqxquJrbp4jk2sQyVXL1RGPkHWYjc0TmDks6";
            var seedCreationDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc);

            List<AuthAccount> authAccounts =
            [
                new AuthAccount
                {
                    Id = 2,
                    Provider = ProviderEnum.Email,
                    ProviderUserId = string.Empty,
                    Email = "manager@example.com",
                    Phone = "0360000002",
                    Password = defaultPasswordHash,
                    Status = AuthStatusEnum.Verified,
                    UserId = 2,
                    CreationDate = seedCreationDate
                },
                new AuthAccount
                {
                    Id = 3,
                    Provider = ProviderEnum.Email,
                    ProviderUserId = string.Empty,
                    Email = "patient@example.com",
                    Phone = "0360000003",
                    Password = defaultPasswordHash,
                    Status = AuthStatusEnum.Verified,
                    UserId = 3,
                    CreationDate = seedCreationDate
                },
                new AuthAccount
                {
                    Id = 4,
                    Provider = ProviderEnum.Email,
                    ProviderUserId = string.Empty,
                    Email = "doctor@example.com",
                    Phone = "0360000004",
                    Password = defaultPasswordHash,
                    Status = AuthStatusEnum.Verified,
                    UserId = 4,
                    CreationDate = seedCreationDate
                },
                new AuthAccount
                {
                    Id = 5,
                    Provider = ProviderEnum.Email,
                    ProviderUserId = string.Empty,
                    Email = "nurse@example.com",
                    Phone = "0360000005",
                    Password = defaultPasswordHash,
                    Status = AuthStatusEnum.Verified,
                    UserId = 5,
                    CreationDate = seedCreationDate
                },
                new AuthAccount
                {
                    Id = 6,
                    Provider = ProviderEnum.Email,
                    ProviderUserId = string.Empty,
                    Email = "hr@example.com",
                    Phone = "0360000006",
                    Password = defaultPasswordHash,
                    Status = AuthStatusEnum.Verified,
                    UserId = 6,
                    CreationDate = seedCreationDate
                },
                new AuthAccount
                {
                    Id = 7,
                    Provider = ProviderEnum.Email,
                    ProviderUserId = string.Empty,
                    Email = "pharmacist@example.com",
                    Phone = "0360000007",
                    Password = defaultPasswordHash,
                    Status = AuthStatusEnum.Verified,
                    UserId = 7,
                    CreationDate = seedCreationDate
                },
                new AuthAccount
                {
                    Id = 8,
                    Provider = ProviderEnum.Email,
                    ProviderUserId = string.Empty,
                    Email = "accountant@example.com",
                    Phone = "0360000008",
                    Password = defaultPasswordHash,
                    Status = AuthStatusEnum.Verified,
                    UserId = 8,
                    CreationDate = seedCreationDate
                },
                new AuthAccount
                {
                    Id = 9,
                    Provider = ProviderEnum.Email,
                    ProviderUserId = string.Empty,
                    Email = "marketer@example.com",
                    Phone = "0360000009",
                    Password = defaultPasswordHash,
                    Status = AuthStatusEnum.Verified,
                    UserId = 9,
                    CreationDate = seedCreationDate
                },
                new AuthAccount
                {
                    Id = 11,
                    Provider = ProviderEnum.Email,
                    ProviderUserId = string.Empty,
                    Email = "receptionist@example.com",
                    Phone = "0360000011",
                    Password = defaultPasswordHash,
                    Status = AuthStatusEnum.Verified,
                    UserId = 11,
                    CreationDate = seedCreationDate
                },
                new AuthAccount
                {
                    Id = 12,
                    Provider = ProviderEnum.Email,
                    ProviderUserId = string.Empty,
                    Email = "hotlineagent@example.com",
                    Phone = "0360000012",
                    Password = defaultPasswordHash,
                    Status = AuthStatusEnum.Verified,
                    UserId = 12,
                    CreationDate = seedCreationDate
                },
                new AuthAccount
                {
                    Id = 13,
                    Provider = ProviderEnum.Email,
                    ProviderUserId = string.Empty,
                    Email = "inventoryclerk@example.com",
                    Phone = "0360000013",
                    Password = defaultPasswordHash,
                    Status = AuthStatusEnum.Verified,
                    UserId = 13,
                    CreationDate = seedCreationDate
                }
            ];

            for (var specialtyId = 1; specialtyId <= 9; specialtyId++)
            {
                var doctorId = 13 + specialtyId;

                authAccounts.Add(new AuthAccount
                {
                    Id = doctorId,
                    Provider = ProviderEnum.Email,
                    ProviderUserId = string.Empty,
                    Email = $"doctor{doctorId}@example.com",
                    Phone = $"0361{doctorId:000000}",
                    Password = defaultPasswordHash,
                    Status = AuthStatusEnum.Verified,
                    UserId = doctorId,
                    CreationDate = seedCreationDate
                });
            }

            for (var patientId = 39; patientId <= 63; patientId++)
            {
                authAccounts.Add(new AuthAccount
                {
                    Id = patientId,
                    Provider = ProviderEnum.Email,
                    ProviderUserId = string.Empty,
                    Email = $"patient{patientId}@example.com",
                    Phone = $"0362{patientId:000000}",
                    Password = defaultPasswordHash,
                    Status = AuthStatusEnum.Verified,
                    UserId = patientId,
                    CreationDate = seedCreationDate
                });
            }

            modelBuilder.Entity<AuthAccount>().HasData(authAccounts.ToArray());

            return modelBuilder;
        }
    }
}
