using WorkScheduleSvc.DAL.Enums;

namespace WorkScheduleSvc.BLL.Interfaces
{
    public interface IWorkScheduleAssignmentValidator
    {
        void ValidateStaffIds(List<int> staffIds);
        LocationTypeEnum ValidateLocationType(string? locationType, string propertyName);
        Task ValidateWorkScheduleMutableAsync(int workScheduleId, string action);
    }
}
