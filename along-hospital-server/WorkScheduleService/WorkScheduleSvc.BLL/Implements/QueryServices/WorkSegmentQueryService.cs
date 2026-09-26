using AutoMapper;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using WorkScheduleSvc.BLL.DTOs.WorkScheduleAssignmentDTOs;
using WorkScheduleSvc.BLL.DTOs.WorkSegmentDTOs;
using WorkScheduleSvc.BLL.Interfaces.Querys;
using WorkScheduleSvc.DAL.Enums;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.BLL.Implements.QueryServices
{
    public class WorkSegmentQueryService(
        IUnitOfWork unitOfWork,
        IMapper mapper) : IWorkSegmentQueryService
    {
        private readonly IGenericRepository<WorkScheduleAssignment> _workScheduleAssignmentRepository = unitOfWork.Repository<WorkScheduleAssignment>();
        private readonly IMapper _mapper = mapper;

        public WorkSegmentPeriodDTO NormalizePeriod(DateTime periodStart, DateTime periodEnd)
        {
            if (periodEnd.TimeOfDay == TimeSpan.Zero)
            {
                periodEnd = periodEnd.Date.AddDays(1).AddTicks(-1);
            }

            return new WorkSegmentPeriodDTO
            {
                PeriodStart = periodStart,
                PeriodEnd = periodEnd
            };
        }

        public async Task<List<GetWorkScheduleAssignmentForSegmentDTO>> GetAssignmentsForGenerationAsync(WorkSegmentPeriodDTO periodDTO)
        {
            var workScheduleAssignments = await _workScheduleAssignmentRepository.GetAllAsync(
                x => x.WorkSchedule != null
                     && x.WorkSchedule.WorkDate >= DateOnly.FromDateTime(periodDTO.PeriodStart)
                     && x.WorkSchedule.WorkDate <= DateOnly.FromDateTime(periodDTO.PeriodEnd)
                     && x.WorkSchedule.WorkScheduleStatus != WorkScheduleStatusEnum.Finalized,
                includes:
                [
                    nameof(WorkScheduleAssignment.WorkSchedule),
                    $"{nameof(WorkScheduleAssignment.WorkSchedule)}.{nameof(WorkSchedule.Shift)}"
                ]);

            return _mapper.Map<List<GetWorkScheduleAssignmentForSegmentDTO>>(workScheduleAssignments);
        }
    }
}
