using WorkScheduleSvc.BLL.DTOs.WorkSegmentDTOs;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.BLL.Interfaces.Validators
{
    public interface IWorkSegmentValidator
    {
        void ValidatePeriod(WorkSegmentPeriodDTO periodDTO);
        void ValidateMutableStatus(WorkSchedule workSchedule, string action);
    }
}
