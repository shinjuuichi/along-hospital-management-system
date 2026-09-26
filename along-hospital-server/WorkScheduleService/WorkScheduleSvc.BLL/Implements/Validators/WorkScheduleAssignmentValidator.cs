using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Utils;
using WorkScheduleSvc.BLL.Interfaces;
using WorkScheduleSvc.DAL.Enums;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.BLL.Implements.Validators
{
    public class WorkScheduleAssignmentValidator(IUnitOfWork unitOfWork) : IWorkScheduleAssignmentValidator
    {
        private readonly IGenericRepository<WorkSchedule> _workScheduleRepository = unitOfWork.Repository<WorkSchedule>();

        public void ValidateStaffIds(List<int> staffIds)
        {
            if (staffIds.Count == 0)
            {
                throw new ValidationFailureException("StaffIds", "StaffIds is required.");
            }
        }

        public LocationTypeEnum ValidateLocationType(string? locationType, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(locationType)
                || !EnumUtil.TryParse(locationType, out LocationTypeEnum location))
            {
                throw new ValidationFailureException(propertyName, "LocationType is invalid.");
            }

            return location;
        }

        public async Task ValidateWorkScheduleMutableAsync(int workScheduleId, string action)
        {
            var workSchedule = await _workScheduleRepository.GetByIdAsync(workScheduleId)
                ?? throw new DataNotFoundException(typeof(WorkSchedule), workScheduleId);

            if (workSchedule.WorkScheduleStatus != WorkScheduleStatusEnum.Draft
                && workSchedule.WorkScheduleStatus != WorkScheduleStatusEnum.Published)
            {
                throw new ValidationFailureException(
                    "Status",
                    $"Work schedule assignment can only be {action} when status is Draft or Published.");
            }
        }
    }
}
