using SharedLibrary.Commons.Exceptions;
using WorkScheduleSvc.BLL.DTOs.WorkSegmentDTOs;
using WorkScheduleSvc.BLL.Interfaces.Validators;
using WorkScheduleSvc.DAL.Enums;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.BLL.Implements.Validators
{
    public class WorkSegmentValidator : IWorkSegmentValidator
    {
        public void ValidatePeriod(WorkSegmentPeriodDTO periodDTO)
        {
            if (periodDTO.PeriodEnd <= periodDTO.PeriodStart)
            {
                throw new InvalidDataException("Period end must be greater than period start.");
            }
        }

        public void ValidateMutableStatus(WorkSchedule workSchedule, string action)
        {
            if (workSchedule.WorkScheduleStatus == WorkScheduleStatusEnum.Finalized)
            {
                throw new ValidationFailureException(
                    nameof(WorkSchedule.WorkScheduleStatus),
                    $"Work segment can only be {action} when work schedule is Draft, Published, or Locked.");
            }
        }
    }
}
