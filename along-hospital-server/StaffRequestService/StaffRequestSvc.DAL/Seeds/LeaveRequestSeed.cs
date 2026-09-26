using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;
using StaffRequestSvc.DAL.Enums;
using StaffRequestSvc.DAL.Models;

namespace StaffRequestSvc.DAL.Seeds;

public class LeaveRequestSeed : ISeedBuilder
{
    public int Priority => 0;

    public ModelBuilder Seed(ModelBuilder modelBuilder)
    {
        var seedDate = new DateTime(2025, 01, 01, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<LeaveRequest>().HasData(
            new LeaveRequest
            {
                Id = 1,
                LeaveType = LeaveTypeEnum.Annual,
                FromDate = new DateOnly(2026, 3, 10),
                ToDate = new DateOnly(2026, 3, 12),
                Reason = "Family vacation",
                Status = RequestStatusEnum.Approved,
                DecidedAt = new DateTime(2026, 3, 5, 10, 0, 0, DateTimeKind.Utc),
                LeaveUnit = LeaveUnitEnum.Day,
                DecidedBy = 1,
                CreatedBy = 3,
                CreationDate = seedDate
            },
            new LeaveRequest
            {
                Id = 2,
                LeaveType = LeaveTypeEnum.Sick,
                FromDate = new DateOnly(2026, 3, 15),
                ToDate = new DateOnly(2026, 3, 15),
                Reason = "Medical checkup",
                Status = RequestStatusEnum.Pending,
                LeaveUnit = LeaveUnitEnum.Day,
                CreatedBy = 4,
                CreationDate = seedDate
            },
            new LeaveRequest
            {
                Id = 3,
                LeaveType = LeaveTypeEnum.Personal,
                FromDate = new DateOnly(2026, 3, 20),
                ToDate = new DateOnly(2026, 3, 20),
                Reason = "Personal errand",
                Status = RequestStatusEnum.Rejected,
                DecidedAt = new DateTime(2026, 3, 18, 14, 0, 0, DateTimeKind.Utc),
                LeaveUnit = LeaveUnitEnum.Shift,
                ShiftId = 1,
                DecidedBy = 1,
                CreatedBy = 5,
                CreationDate = seedDate
            },
            new LeaveRequest
            {
                Id = 4,
                LeaveType = LeaveTypeEnum.Maternity,
                FromDate = new DateOnly(2026, 4, 1),
                ToDate = new DateOnly(2026, 6, 30),
                Reason = "Maternity leave",
                Status = RequestStatusEnum.Approved,
                DecidedAt = new DateTime(2026, 3, 25, 9, 0, 0, DateTimeKind.Utc),
                LeaveUnit = LeaveUnitEnum.Day,
                DecidedBy = 1,
                CreatedBy = 6,
                CreationDate = seedDate
            },
            new LeaveRequest
            {
                Id = 5,
                LeaveType = LeaveTypeEnum.Annual,
                FromDate = new DateOnly(2026, 3, 25),
                ToDate = new DateOnly(2026, 3, 28),
                Reason = "Travel plan",
                Status = RequestStatusEnum.Pending,
                LeaveUnit = LeaveUnitEnum.Day,
                CreatedBy = 7,
                CreationDate = seedDate
            }
        );

        return modelBuilder;
    }
}
