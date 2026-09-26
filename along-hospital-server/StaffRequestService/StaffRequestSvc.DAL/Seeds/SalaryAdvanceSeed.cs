using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;
using StaffRequestSvc.DAL.Enums;
using StaffRequestSvc.DAL.Models;

namespace StaffRequestSvc.DAL.Seeds
{
    public class SalaryAdvanceSeed : ISeedBuilder
    {
        public int Priority => 0;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc);
            const int doctorId = 4;
            const int managerId = 2;
            var salaryAdvances = new List<SalaryAdvance>
            {
                new() {
                    Id = 1,
                    Amount = 88,
                    Reason = "Pending advance for family expense",
                    Status = SalaryAdvanceStatusEnum.Pending,
                    CreatedBy = doctorId,
                    CreationDate = seedDate
                },
                new() {
                    Id = 2,
                    Amount = 128,
                    Reason = "Approved advance for rent payment",
                    Status = SalaryAdvanceStatusEnum.Approved,
                    CreatedBy = doctorId,
                    CreationDate = seedDate.AddDays(1),
                    DecidedAt = seedDate.AddDays(1),
                    DecidedBy = managerId
                },
                new() {
                    Id = 3,
                    Amount = 72,
                    Reason = "Rejected advance request",
                    Status = SalaryAdvanceStatusEnum.Rejected,
                    CreatedBy = doctorId,
                    CreationDate = seedDate.AddDays(2),
                    DecidedAt = seedDate.AddDays(2),
                    DecidedBy = managerId
                },
                new() {
                    Id = 4,
                    Amount = 60,
                    Reason = "Cancelled by doctor before approval",
                    Status = SalaryAdvanceStatusEnum.Cancelled,
                    CreatedBy = doctorId,
                    CreationDate = seedDate.AddDays(3)
                },
                new() {
                    Id = 5,
                    Amount = 140,
                    Reason = "Disbursed advance linked to paid payroll",
                    Status = SalaryAdvanceStatusEnum.Disbursed,
                    CreatedBy = doctorId,
                    CreationDate = seedDate.AddDays(4),
                    DecidedAt = seedDate.AddDays(4),
                    DecidedBy = managerId,
                    PayrollId = 2
                }
            };

            var nextId = 6;
            foreach (var staffId in this.GetAllStaffIds().Where(x => x != doctorId))
            {
                var creationDate = seedDate.AddDays(nextId * 2);
                var status = this.GetStatus(staffId);
                var salaryAdvance = new SalaryAdvance
                {
                    Id = nextId,
                    Amount = this.GetAmount(staffId),
                    Reason = this.GetReason(staffId, status),
                    Status = status,
                    CreatedBy = staffId,
                    CreationDate = creationDate
                };

                if (status is SalaryAdvanceStatusEnum.Approved
                    or SalaryAdvanceStatusEnum.Rejected
                    or SalaryAdvanceStatusEnum.Disbursed)
                {
                    salaryAdvance.DecidedAt = creationDate.AddDays(2);
                    salaryAdvance.DecidedBy = this.GetDecidedBy(staffId);
                }

                if (status == SalaryAdvanceStatusEnum.Disbursed)
                {
                    salaryAdvance.PayrollId = this.GetPayrollId(staffId);
                }

                salaryAdvances.Add(salaryAdvance);
                nextId++;
            }

            modelBuilder.Entity<SalaryAdvance>().HasData(salaryAdvances.ToArray());

            return modelBuilder;
        }

        private List<int> GetAllStaffIds()
        {
            var staffIds = new List<int> { 2, 4, 5, 6, 7, 8, 9, 11, 12, 13 };
            staffIds.AddRange(Enumerable.Range(14, 9));
            return staffIds;
        }

        private SalaryAdvanceStatusEnum GetStatus(int staffId)
        {
            return (staffId % 5) switch
            {
                0 => SalaryAdvanceStatusEnum.Pending,
                1 => SalaryAdvanceStatusEnum.Approved,
                2 => SalaryAdvanceStatusEnum.Rejected,
                3 => SalaryAdvanceStatusEnum.Cancelled,
                _ => SalaryAdvanceStatusEnum.Disbursed
            };
        }

        private double GetAmount(int staffId)
        {
            return staffId switch
            {
                2 => 260,
                5 => 88,
                6 => 180,
                7 => 112,
                8 => 128,
                9 => 100,
                11 => 72,
                12 => 80,
                13 => 92,
                _ => 140 + ((staffId - 14) % 5) * 24
            };
        }

        private string GetReason(int staffId, SalaryAdvanceStatusEnum status)
        {
            var roleReason = staffId switch
            {
                2 => "Advance for monthly leadership expenses",
                5 => "Advance for nursing family support",
                6 => "Advance for annual training fee",
                7 => "Advance for pharmacy licensing cost",
                8 => "Advance for household payment plan",
                9 => "Advance for campaign travel expense",
                11 => "Advance for front desk living expense",
                12 => "Advance for hotline family emergency",
                13 => "Advance for warehouse transportation cost",
                _ => $"Advance for specialist practice expense #{staffId}"
            };

            return status switch
            {
                SalaryAdvanceStatusEnum.Pending => $"{roleReason} pending review",
                SalaryAdvanceStatusEnum.Approved => $"{roleReason} approved by management",
                SalaryAdvanceStatusEnum.Rejected => $"{roleReason} rejected after review",
                SalaryAdvanceStatusEnum.Cancelled => $"{roleReason} cancelled by staff",
                SalaryAdvanceStatusEnum.Disbursed => $"{roleReason} disbursed with payroll",
                _ => roleReason
            };
        }

        private int GetDecidedBy(int staffId)
        {
            return staffId == 2 ? 6 : 2;
        }

        private int GetPayrollId(int staffId)
        {
            var staffIds = this.GetAllStaffIds();
            var staffIndex = staffIds.IndexOf(staffId);
            return 4 + (staffIndex * 2);
        }
    }
}
