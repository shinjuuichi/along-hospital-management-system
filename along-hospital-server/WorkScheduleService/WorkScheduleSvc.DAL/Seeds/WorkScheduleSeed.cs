using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;
using WorkScheduleSvc.DAL.Enums;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.DAL.Seeds
{
    public class WorkScheduleSeed : ISeedBuilder
    {
        public int Priority => 1;
        private static readonly DateOnly CurrentMonthStart = new(2026, 4, 1);
        private static readonly DateOnly NextMonthEnd = new(2026, 5, 31);

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2026, 3, 18, 19, 17, 0, DateTimeKind.Utc);
            var workSchedules = new List<WorkSchedule>
            {
                new WorkSchedule
                {
                    Id = 1,
                    WorkDate = new DateOnly(2026, 4, 6),
                    ShiftId = 1,
                    WorkScheduleTemplateId = this.GetWorkScheduleTemplateId(new DateOnly(2026, 4, 6)),
                    WorkScheduleStatus = WorkScheduleStatusEnum.Draft,
                    CreationDate = seedDate,
                    CreatedBy = 6
                },
                new WorkSchedule
                {
                    Id = 2,
                    WorkDate = new DateOnly(2026, 4, 7),
                    ShiftId = 2,
                    WorkScheduleTemplateId = this.GetWorkScheduleTemplateId(new DateOnly(2026, 4, 7)),
                    WorkScheduleStatus = WorkScheduleStatusEnum.Draft,
                    CreationDate = seedDate.AddMinutes(1),
                    CreatedBy = 6
                },
                new WorkSchedule
                {
                    Id = 3,
                    WorkDate = new DateOnly(2026, 4, 8),
                    ShiftId = 3,
                    WorkScheduleTemplateId = this.GetWorkScheduleTemplateId(new DateOnly(2026, 4, 8)),
                    WorkScheduleStatus = WorkScheduleStatusEnum.Draft,
                    CreationDate = seedDate.AddMinutes(2),
                    CreatedBy = 6
                },
                new WorkSchedule
                {
                    Id = 4,
                    WorkDate = new DateOnly(2026, 4, 9),
                    ShiftId = 4,
                    WorkScheduleTemplateId = this.GetWorkScheduleTemplateId(new DateOnly(2026, 4, 9)),
                    WorkScheduleStatus = WorkScheduleStatusEnum.Draft,
                    CreationDate = seedDate.AddMinutes(3),
                    CreatedBy = 6
                }
            };
            var existingScheduleKeys = workSchedules
                .Select(x => this.GetScheduleKey(x.WorkDate, x.ShiftId))
                .ToHashSet();
            var nextId = 5;

            for (var workDate = CurrentMonthStart; workDate <= NextMonthEnd; workDate = workDate.AddDays(1))
            {
                if (workDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    continue;
                }

                foreach (var shiftId in this.GetShiftIds(workDate))
                {
                    var scheduleKey = this.GetScheduleKey(workDate, shiftId);
                    if (existingScheduleKeys.Contains(scheduleKey))
                    {
                        continue;
                    }

                    workSchedules.Add(new WorkSchedule
                    {
                        Id = nextId,
                        WorkDate = workDate,
                        ShiftId = shiftId,
                        WorkScheduleTemplateId = this.GetWorkScheduleTemplateId(workDate),
                        WorkScheduleStatus = workDate.Month == 4
                            ? WorkScheduleStatusEnum.Published
                            : WorkScheduleStatusEnum.Draft,
                        CreationDate = seedDate.AddMinutes(nextId),
                        CreatedBy = 6
                    });

                    existingScheduleKeys.Add(scheduleKey);
                    nextId++;
                }
            }

            modelBuilder.Entity<WorkSchedule>().HasData(workSchedules.ToArray());

            return modelBuilder;
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

        private bool IsHoliday(DateOnly workDate)
        {
            return (workDate.Month == 4 && workDate.Day == 30)
                || (workDate.Month == 5 && workDate.Day == 1);
        }

        private string GetScheduleKey(DateOnly workDate, int shiftId)
        {
            return $"{workDate:yyyy-MM-dd}-{shiftId}";
        }

        private int? GetWorkScheduleTemplateId(DateOnly workDate)
        {
            return workDate.Month switch
            {
                4 => 1,
                5 => 2,
                _ => null
            };
        }
    }
}
