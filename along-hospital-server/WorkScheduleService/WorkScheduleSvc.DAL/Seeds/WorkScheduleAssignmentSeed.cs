using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;
using WorkScheduleSvc.DAL.Enums;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.DAL.Seeds
{
    public class WorkScheduleAssignmentSeed : ISeedBuilder
    {
        public int Priority => 2;
        private static readonly DateOnly CurrentMonthStart = new(2026, 4, 1);
        private static readonly DateOnly NextMonthEnd = new(2026, 5, 31);

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2026, 3, 18, 19, 18, 0, DateTimeKind.Utc);
            var assignments = new List<WorkScheduleAssignment>
            {
                new WorkScheduleAssignment
                {
                    Id = 1,
                    WorkScheduleId = 1,
                    StaffId = 5,
                    LocationId = 1,
                    LocationType = LocationTypeEnum.Room,
                    CreationDate = seedDate,
                    CreatedBy = 6
                },
                new WorkScheduleAssignment
                {
                    Id = 2,
                    WorkScheduleId = 1,
                    StaffId = 15,
                    LocationId = 2,
                    LocationType = LocationTypeEnum.Room,
                    CreationDate = seedDate,
                    CreatedBy = 6
                },
                new WorkScheduleAssignment
                {
                    Id = 3,
                    WorkScheduleId = 2,
                    StaffId = 16,
                    LocationId = 3,
                    LocationType = LocationTypeEnum.Room,
                    CreationDate = seedDate.AddMinutes(1),
                    CreatedBy = 6
                },
                new WorkScheduleAssignment
                {
                    Id = 4,
                    WorkScheduleId = 2,
                    StaffId = 14,
                    LocationId = 1,
                    LocationType = LocationTypeEnum.TeleRoom,
                    CreationDate = seedDate.AddMinutes(1),
                    CreatedBy = 6
                },
                new WorkScheduleAssignment
                {
                    Id = 5,
                    WorkScheduleId = 3,
                    StaffId = 4,
                    LocationId = 4,
                    LocationType = LocationTypeEnum.TeleRoom,
                    CreationDate = seedDate.AddMinutes(2),
                    CreatedBy = 6
                },
                new WorkScheduleAssignment
                {
                    Id = 6,
                    WorkScheduleId = 3,
                    StaffId = 21,
                    LocationId = 8,
                    LocationType = LocationTypeEnum.Room,
                    CreationDate = seedDate.AddMinutes(2),
                    CreatedBy = 6
                },
                new WorkScheduleAssignment
                {
                    Id = 8,
                    WorkScheduleId = 4,
                    StaffId = 22,
                    LocationId = 9,
                    LocationType = LocationTypeEnum.TeleRoom,
                    CreationDate = seedDate.AddMinutes(3),
                    CreatedBy = 6
                }
            };

            var existingAssignmentKeys = assignments
                .Select(x => this.GetAssignmentKey(x.WorkScheduleId, x.StaffId))
                .ToHashSet();
            var scheduleRefs = this.BuildScheduleRefs();
            var scheduleDateLookup = scheduleRefs.ToDictionary(x => x.Id, x => x.WorkDate);
            var scheduleLookup = scheduleRefs.ToDictionary(
                x => this.GetScheduleKey(x.WorkDate, x.ShiftId),
                x => x.Id);
            var existingStaffDateKeys = assignments
                .Select(x => this.GetStaffDateKey(scheduleDateLookup[x.WorkScheduleId], x.StaffId))
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

                    assignments.Add(new WorkScheduleAssignment
                    {
                        Id = nextId,
                        WorkScheduleId = workScheduleId,
                        StaffId = staffId,
                        LocationId = location.Value.LocationId,
                        LocationType = location.Value.LocationType,
                        CreationDate = seedDate.AddMinutes(nextId),
                        CreatedBy = 6
                    });

                    existingAssignmentKeys.Add(assignmentKey);
                    existingStaffDateKeys.Add(staffDateKey);
                    nextId++;
                }
            }

            modelBuilder.Entity<WorkScheduleAssignment>().HasData(assignments.ToArray());

            return modelBuilder;
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
                new ScheduleRef(1, new DateOnly(2026, 4, 6), 1),
                new ScheduleRef(2, new DateOnly(2026, 4, 7), 2),
                new ScheduleRef(3, new DateOnly(2026, 4, 8), 3),
                new ScheduleRef(4, new DateOnly(2026, 4, 9), 4)
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
    }
}
