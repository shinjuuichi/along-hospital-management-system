using Microsoft.EntityFrameworkCore;
using RecruitmentSvc.DAL.Enums;
using RecruitmentSvc.DAL.Models;
using SharedLibrary.Base.Data;

namespace RecruitmentSvc.DAL.Seeds
{
    public class JobApplicationSeed : ISeedBuilder
    {
        public int Priority => 2;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JobApplication>().HasData(
                new JobApplication
                {
                    Id = 1,
                    Name = "John Smith",
                    Email = "john.smith@email.com",
                    Phone = "0912345678",
                    DateOfBirth = new DateOnly(1990, 5, 15),
                    Address = "123 Main St, District 1, HCMC",
                    Gender = GenderEnum.Male,
                    ApplyDate = new DateOnly(2025, 1, 15),
                    ApplicationStatus = JobApplicationStatusEnum.Passed,
                    CVUrl = "JobApplication/ba45fada8c194f78b76b3cc74e907b9d.pdf",
                    JobPostingId = 1
                },
                new JobApplication
                {
                    Id = 2,
                    Name = "Mary Johnson",
                    Email = "mquwntran04@gmail.com",
                    Phone = "0912345679",
                    DateOfBirth = new DateOnly(1985, 8, 22),
                    Address = "456 Oak Ave, District 3, HCMC",
                    Gender = GenderEnum.Female,
                    ApplyDate = new DateOnly(2025, 1, 20),
                    ApplicationStatus = JobApplicationStatusEnum.Interviewing,
                    CVUrl = "JobApplication/ba45fada8c194f78b76b3cc74e907b9d.pdf",
                    JobPostingId = 1
                },
                new JobApplication
                {
                    Id = 3,
                    Name = "David Lee",
                    Email = "david.lee@email.com",
                    Phone = "0912345680",
                    DateOfBirth = new DateOnly(1992, 3, 10),
                    Address = "789 Pine Rd, District 5, HCMC",
                    Gender = GenderEnum.Male,
                    ApplyDate = new DateOnly(2025, 2, 1),
                    ApplicationStatus = JobApplicationStatusEnum.Passed,
                    CVUrl = "JobApplication/717a3c36949d4e539b18afb15756e89f.pdf",
                    JobPostingId = 2
                },
                new JobApplication
                {
                    Id = 4,
                    Name = "Sarah Wilson",
                    Email = "sarah.wilson@email.com",
                    Phone = "0912345681",
                    DateOfBirth = new DateOnly(1988, 11, 25),
                    Address = "321 Elm St, District 7, HCMC",
                    Gender = GenderEnum.Female,
                    ApplyDate = new DateOnly(2025, 2, 5),
                    ApplicationStatus = JobApplicationStatusEnum.Interviewing,
                    CVUrl = "JobApplication/ba45fada8c194f78b76b3cc74e907b9d.pdf",
                    JobPostingId = 2
                },
                new JobApplication
                {
                    Id = 5,
                    Name = "Michael Brown",
                    Email = "mquwntran04@gmail.com",
                    Phone = "0912345682",
                    DateOfBirth = new DateOnly(1995, 7, 8),
                    Address = "654 Maple Dr, District 9, HCMC",
                    Gender = GenderEnum.Male,
                    ApplyDate = new DateOnly(2025, 2, 10),
                    ApplicationStatus = JobApplicationStatusEnum.Applied,
                    CVUrl = "JobApplication/ba45fada8c194f78b76b3cc74e907b9d.pdf",
                    JobPostingId = 3
                }
            );

            return modelBuilder;
        }
    }
}
