using Microsoft.EntityFrameworkCore;
using RecruitmentSvc.DAL.Enums;
using RecruitmentSvc.DAL.Models;
using SharedLibrary.Base.Data;
using SharedLibrary.Enums;

namespace RecruitmentSvc.DAL.Seeds
{
    public class JobPostingSeed : ISeedBuilder
    {
        public int Priority => 0;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var seedCreationDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<JobPosting>().HasData(
                new JobPosting
                {
                    Id = 1,
                    Title = "General Accountant - Xuan Phuong, Hanoi",
                    Role = RoleEnum.Accountant,
                    Description = "Manage accounting records, invoices, taxes, and financial reports for daily operations.",
                    Requirement = "Accounting or finance degree, 2-3 years of experience, and accounting software proficiency.",
                    Benefit = "Salary 15-20 million VND, insurance, leave, bonuses, and team activities.",
                    EmploymentType = EmploymentTypeEnum.FullTime,
                    SalaryMin = 50000,
                    CloseDate = new DateOnly(2029, 12, 31),
                    Status = JobPostingStatusEnum.Open,
                    CreationDate = seedCreationDate
                },
                new JobPosting
                {
                    Id = 2,
                    Title = "Project Manager (Chinese Communication)",
                    Role = RoleEnum.Manager,
                    Description = "Coordinate client projects, licensing procedures, and progress reporting with Chinese-speaking stakeholders.",
                    Requirement = "Bachelor's degree, 3+ years of project management experience, and professional Chinese communication.",
                    Benefit = "Salary 15-25 million VND, project bonuses, insurance, leave, and company welfare programs.",
                    EmploymentType = EmploymentTypeEnum.FullTime,
                    SalaryMin = 30000,
                    CloseDate = new DateOnly(2029, 6, 30),
                    Status = JobPostingStatusEnum.Draft,
                    CreationDate = seedCreationDate
                },
                new JobPosting
                {
                    Id = 3,
                    Title = "Marketing Staff (Product Fresher)",
                    Role = RoleEnum.Marketer,
                    Description = "Research products and customers, optimize product pages, and coordinate creative marketing content.",
                    Requirement = "Marketing-related degree, basic English, research skills, AI tool familiarity, and creativity.",
                    Benefit = "Base salary 8-10 million VND, profit bonus, yearly review, and a dynamic workplace.",
                    EmploymentType = EmploymentTypeEnum.PartTime,
                    SalaryMin = 15000,
                    CloseDate = new DateOnly(2029, 3, 31),
                    Status = JobPostingStatusEnum.Open,
                    CreationDate = seedCreationDate
                }
            );

            return modelBuilder;
        }
    }
}
