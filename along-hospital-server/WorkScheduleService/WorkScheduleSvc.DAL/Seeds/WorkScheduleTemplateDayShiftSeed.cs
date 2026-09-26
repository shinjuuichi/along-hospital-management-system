using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;
using WorkScheduleSvc.DAL.Enums;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.DAL.Seeds
{
    public class WorkScheduleTemplateDayShiftSeed : ISeedBuilder
    {
        public int Priority => 1;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            var seedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var templateDayShifts = new List<WorkScheduleTemplateDayShift>();
            var nextId = 1;

            this.AddMonthlyTemplateDayShifts(templateDayShifts, ref nextId, 1, seedDate);
            this.AddMonthlyTemplateDayShifts(templateDayShifts, ref nextId, 2, seedDate);
            this.AddFutureTemplateDayShifts(templateDayShifts, ref nextId, 3, seedDate);

            modelBuilder.Entity<WorkScheduleTemplateDayShift>().HasData(templateDayShifts.ToArray());

            return modelBuilder;
        }

        private void AddMonthlyTemplateDayShifts(
            List<WorkScheduleTemplateDayShift> templateDayShifts,
            ref int nextId,
            int templateId,
            DateTime seedDate)
        {
            this.AddDayShiftSeeds(templateDayShifts, ref nextId, templateId, DayOfWeekEnum.Monday, [1, 2, 3], seedDate);
            this.AddDayShiftSeeds(templateDayShifts, ref nextId, templateId, DayOfWeekEnum.Tuesday, [1, 2, 3], seedDate);
            this.AddDayShiftSeeds(templateDayShifts, ref nextId, templateId, DayOfWeekEnum.Wednesday, [1, 2, 3], seedDate);
            this.AddDayShiftSeeds(templateDayShifts, ref nextId, templateId, DayOfWeekEnum.Thursday, [1, 2, 3, 4], seedDate);
            this.AddDayShiftSeeds(templateDayShifts, ref nextId, templateId, DayOfWeekEnum.Friday, [1, 2, 3], seedDate);
            this.AddDayShiftSeeds(templateDayShifts, ref nextId, templateId, DayOfWeekEnum.Saturday, [1, 4], seedDate);
        }

        private void AddFutureTemplateDayShifts(
            List<WorkScheduleTemplateDayShift> templateDayShifts,
            ref int nextId,
            int templateId,
            DateTime seedDate)
        {
            this.AddDayShiftSeeds(templateDayShifts, ref nextId, templateId, DayOfWeekEnum.Monday, [1, 2], seedDate);
            this.AddDayShiftSeeds(templateDayShifts, ref nextId, templateId, DayOfWeekEnum.Tuesday, [1, 2], seedDate);
            this.AddDayShiftSeeds(templateDayShifts, ref nextId, templateId, DayOfWeekEnum.Wednesday, [1, 2], seedDate);
            this.AddDayShiftSeeds(templateDayShifts, ref nextId, templateId, DayOfWeekEnum.Thursday, [1, 2], seedDate);
            this.AddDayShiftSeeds(templateDayShifts, ref nextId, templateId, DayOfWeekEnum.Friday, [1, 2], seedDate);
        }

        private void AddDayShiftSeeds(
            List<WorkScheduleTemplateDayShift> templateDayShifts,
            ref int nextId,
            int templateId,
            DayOfWeekEnum dayOfWeek,
            IEnumerable<int> shiftIds,
            DateTime seedDate)
        {
            foreach (var shiftId in shiftIds)
            {
                templateDayShifts.Add(new WorkScheduleTemplateDayShift
                {
                    Id = nextId,
                    DayOfWeek = dayOfWeek,
                    WorkScheduleTemplateId = templateId,
                    ShiftId = shiftId,
                    CreationDate = seedDate
                });
                nextId++;
            }
        }
    }
}
