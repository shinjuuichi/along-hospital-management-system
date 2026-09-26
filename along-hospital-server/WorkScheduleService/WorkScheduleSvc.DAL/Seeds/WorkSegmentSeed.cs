using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;
using WorkScheduleSvc.DAL.Enums;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.DAL.Seeds
{
    public class WorkSegmentSeed : ISeedBuilder
    {
        public int Priority => 3;
        private static readonly DateOnly CurrentMonthStart = new(2026, 4, 1);
        private static readonly DateOnly NextMonthEnd = new(2026, 5, 31);

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2026, 3, 18, 19, 55, 0, DateTimeKind.Utc);
            var workSegments = new List<WorkSegment>();
            var scheduleRefs = this.BuildScheduleRefs();
            var assignments = this.BuildAssignments(scheduleRefs);
            var scheduleLookup = scheduleRefs.ToDictionary(x => x.Id);
            var nextId = 1;

            foreach (var assignment in assignments)
            {
                var schedule = scheduleLookup[assignment.WorkScheduleId];
                var timeRange = this.GetShiftTimeRange(schedule.WorkDate, schedule.ShiftId);
                var status = schedule.ShiftId == 4
                    ? WorkStatusEnum.Overtime
                    : assignment.Id % 6 == 0
                        ? WorkStatusEnum.Absent
                        : WorkStatusEnum.Worked;
                var reason = status switch
                {
                    WorkStatusEnum.Worked => WorkStatusReasonEnum.WorkedFromAttendance,
                    WorkStatusEnum.Overtime => WorkStatusReasonEnum.WorkedFromAttendance,
                    _ => this.GetAbsentReason(assignment.Id)
                };

                workSegments.Add(new WorkSegment
                {
                    Id = nextId,
                    StartTime = timeRange.StartTime,
                    EndTime = timeRange.EndTime,
                    WorkStatus = status,
                    WorkStatusReason = reason,
                    WorkScheduleAssignmentId = assignment.Id,
                    CreationDate = seedDate.AddMinutes(nextId),
                    CreatedBy = 6
                });
                nextId++;
            }

            modelBuilder.Entity<WorkSegment>().HasData(workSegments.ToArray());

            return modelBuilder;
        }

        private List<AssignmentRef> BuildAssignments(List<ScheduleRef> scheduleRefs)
        {
            var assignments = new List<AssignmentRef>
            {
                new(1, 1, 5, 1, LocationTypeEnum.Room),
                new(2, 1, 15, 2, LocationTypeEnum.Room),
                new(3, 2, 16, 3, LocationTypeEnum.Room),
                new(4, 2, 14, 1, LocationTypeEnum.TeleRoom),
                new(5, 3, 4, 4, LocationTypeEnum.TeleRoom),
                new(6, 3, 21, 8, LocationTypeEnum.Room),
                new(8, 4, 22, 9, LocationTypeEnum.TeleRoom)
            };

            var existingAssignmentKeys = assignments
                .Select(x => this.GetAssignmentKey(x.WorkScheduleId, x.StaffId))
                .ToHashSet();
            var scheduleLookup = scheduleRefs
                .ToDictionary(x => this.GetScheduleKey(x.WorkDate, x.ShiftId), x => x.Id);
            var existingStaffDateKeys = assignments
                .Select(x => this.GetStaffDateKey(this.GetSeedWorkDateByScheduleId(x.WorkScheduleId), x.StaffId))
                .ToHashSet();
            var nextId = 9;

            foreach (var workDate in this.GetWorkDates())
            {
                foreach (var staffId in this.GetAssignableStaffIds())
                {
                    var staffDateKey = this.GetStaffDateKey(workDate, staffId);
                    if (existingStaffDateKeys.Contains(staffDateKey))
                    {
                        continue;
                    }

                    var shiftId = this.GetShiftId(staffId, workDate);
                    if (shiftId is null)
                    {
                        continue;
                    }

                    var scheduleKey = this.GetScheduleKey(workDate, shiftId.Value);
                    if (!scheduleLookup.TryGetValue(scheduleKey, out var workScheduleId))
                    {
                        continue;
                    }

                    var location = this.GetLocation(staffId);
                    if (location is null)
                    {
                        continue;
                    }

                    var assignmentKey = this.GetAssignmentKey(workScheduleId, staffId);
                    if (existingAssignmentKeys.Contains(assignmentKey))
                    {
                        continue;
                    }

                    assignments.Add(new AssignmentRef(
                        nextId,
                        workScheduleId,
                        staffId,
                        location.Value.LocationId,
                        location.Value.LocationType));

                    existingAssignmentKeys.Add(assignmentKey);
                    existingStaffDateKeys.Add(staffDateKey);
                    nextId++;
                }
            }

            return assignments;
        }

        private (DateTime StartTime, DateTime EndTime) GetShiftTimeRange(DateOnly workDate, int shiftId)
        {
            return shiftId switch
            {
                1 => (new DateTime(workDate.Year, workDate.Month, workDate.Day, 7, 0, 0, DateTimeKind.Utc),
                    new DateTime(workDate.Year, workDate.Month, workDate.Day, 12, 0, 0, DateTimeKind.Utc)),
                2 => (new DateTime(workDate.Year, workDate.Month, workDate.Day, 12, 0, 0, DateTimeKind.Utc),
                    new DateTime(workDate.Year, workDate.Month, workDate.Day, 17, 0, 0, DateTimeKind.Utc)),
                3 => (new DateTime(workDate.Year, workDate.Month, workDate.Day, 17, 0, 0, DateTimeKind.Utc),
                    new DateTime(workDate.Year, workDate.Month, workDate.Day, 22, 0, 0, DateTimeKind.Utc)),
                _ => (new DateTime(workDate.Year, workDate.Month, workDate.Day, 22, 0, 0, DateTimeKind.Utc),
                    new DateTime(workDate.Year, workDate.Month, workDate.Day, 23, 30, 0, DateTimeKind.Utc))
            };
        }

        private WorkStatusReasonEnum GetAbsentReason(int assignmentId)
        {
            return (assignmentId % 3) switch
            {
                0 => WorkStatusReasonEnum.NoAttendanceLog,
                1 => WorkStatusReasonEnum.MissingCheckIn,
                _ => WorkStatusReasonEnum.MissingCheckOut
            };
        }

        private DateOnly GetSeedWorkDateByScheduleId(int workScheduleId)
        {
            return workScheduleId switch
            {
                1 => new DateOnly(2026, 4, 6),
                2 => new DateOnly(2026, 4, 7),
                3 => new DateOnly(2026, 4, 8),
                4 => new DateOnly(2026, 4, 9),
                _ => CurrentMonthStart
            };
        }

        private List<int> GetAssignableStaffIds()
        {
            var staffIds = new List<int> { 5, 4 };
            staffIds.AddRange(Enumerable.Range(14, 9));
            return staffIds;
        }

        private List<DateOnly> GetWorkDates()
        {
            var workDates = new List<DateOnly>();
            for (var workDate = CurrentMonthStart; workDate <= NextMonthEnd; workDate = workDate.AddDays(1))
            {
                if (workDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    workDates.Add(workDate);
                }
            }

            return workDates;
        }

        private List<ScheduleRef> BuildScheduleRefs()
        {
            var scheduleRefs = new List<ScheduleRef>
            {
                new(1, new DateOnly(2026, 4, 6), 1),
                new(2, new DateOnly(2026, 4, 7), 2),
                new(3, new DateOnly(2026, 4, 8), 3),
                new(4, new DateOnly(2026, 4, 9), 4)
            };
            var existingKeys = scheduleRefs
                .Select(x => this.GetScheduleKey(x.WorkDate, x.ShiftId))
                .ToHashSet();
            var nextId = 5;

            foreach (var workDate in this.GetWorkDates())
            {
                foreach (var shiftId in this.GetShiftIds(workDate))
                {
                    var scheduleKey = this.GetScheduleKey(workDate, shiftId);
                    if (existingKeys.Contains(scheduleKey))
                    {
                        continue;
                    }

                    scheduleRefs.Add(new ScheduleRef(nextId, workDate, shiftId));
                    existingKeys.Add(scheduleKey);
                    nextId++;
                }
            }

            return scheduleRefs;
        }

        private List<int> GetShiftIds(DateOnly workDate)
        {
            if (this.IsHoliday(workDate) || workDate.DayOfWeek == DayOfWeek.Saturday)
            {
                return [1, 4];
            }

            if (workDate.DayOfWeek == DayOfWeek.Thursday)
            {
                return [1, 2, 3, 4];
            }

            return [1, 2, 3];
        }

        private int? GetShiftId(int staffId, DateOnly workDate)
        {
            if (staffId == 5)
            {
                if (this.IsHoliday(workDate) || workDate.DayOfWeek == DayOfWeek.Saturday)
                {
                    return 1;
                }

                return workDate.DayOfWeek is DayOfWeek.Monday or DayOfWeek.Wednesday or DayOfWeek.Friday
                    ? 1
                    : 2;
            }

            var specialtyId = this.GetDoctorSpecialtyId(staffId);
            if (this.UseRoomForDoctor(specialtyId))
            {
                if (this.IsHoliday(workDate) || workDate.DayOfWeek == DayOfWeek.Saturday)
                {
                    return specialtyId % 2 == 0 ? 1 : 4;
                }

                if (workDate.DayOfWeek == DayOfWeek.Thursday)
                {
                    var thursdayShifts = new[] { 1, 2, 3, 4 };
                    return thursdayShifts[(specialtyId + workDate.Day) % thursdayShifts.Length];
                }

                var weekdayShifts = new[] { 1, 2, 3 };
                return weekdayShifts[(specialtyId + workDate.Day) % weekdayShifts.Length];
            }

            if (this.IsHoliday(workDate))
            {
                return null;
            }

            return workDate.DayOfWeek switch
            {
                DayOfWeek.Saturday => 1,
                DayOfWeek.Monday => 1,
                DayOfWeek.Tuesday => 2,
                DayOfWeek.Wednesday => 1,
                DayOfWeek.Thursday => 2,
                DayOfWeek.Friday => 1,
                _ => null
            };
        }

        private (LocationTypeEnum LocationType, int LocationId)? GetLocation(int staffId)
        {
            if (staffId == 5)
            {
                return (LocationTypeEnum.Room, 1);
            }

            var specialtyId = this.GetDoctorSpecialtyId(staffId);
            if (this.UseRoomForDoctor(specialtyId))
            {
                return specialtyId switch
                {
                    2 => (LocationTypeEnum.Room, 2),
                    3 => (LocationTypeEnum.Room, 3),
                    8 => (LocationTypeEnum.Room, 8),
                    _ => null
                };
            }

            return (LocationTypeEnum.TeleRoom, specialtyId);
        }

        private int GetDoctorSpecialtyId(int staffId)
        {
            return staffId == 4 ? 4 : staffId - 13;
        }

        private bool UseRoomForDoctor(int specialtyId)
        {
            return specialtyId is 2 or 3 or 8;
        }

        private bool IsHoliday(DateOnly workDate)
        {
            return (workDate.Month == 4 && workDate.Day == 30)
                || (workDate.Month == 5 && workDate.Day == 1);
        }

        private string GetScheduleKey(DateOnly workDate, int shiftId)
        {
            return $"{workDate:yyyy-MM-dd}-{shiftId}";
        }

        private string GetAssignmentKey(int workScheduleId, int staffId)
        {
            return $"{workScheduleId}-{staffId}";
        }

        private string GetStaffDateKey(DateOnly workDate, int staffId)
        {
            return $"{workDate:yyyy-MM-dd}-{staffId}";
        }

        private class ScheduleRef
        {
            public ScheduleRef(int id, DateOnly workDate, int shiftId)
            {
                Id = id;
                WorkDate = workDate;
                ShiftId = shiftId;
            }

            public int Id { get; }
            public DateOnly WorkDate { get; }
            public int ShiftId { get; }
        }

        private class AssignmentRef
        {
            public AssignmentRef(int id, int workScheduleId, int staffId, int locationId, LocationTypeEnum locationType)
            {
                Id = id;
                WorkScheduleId = workScheduleId;
                StaffId = staffId;
                LocationId = locationId;
                LocationType = locationType;
            }

            public int Id { get; }
            public int WorkScheduleId { get; }
            public int StaffId { get; }
            public int LocationId { get; }
            public LocationTypeEnum LocationType { get; }
        }
    }
}
