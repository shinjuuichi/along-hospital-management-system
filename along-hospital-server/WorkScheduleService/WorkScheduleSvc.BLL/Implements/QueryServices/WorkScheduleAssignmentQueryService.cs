using AutoMapper;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Commons.Filters;
using SharedLibrary.Commons.Results;
using SharedLibrary.Services.Interfaces;
using WorkScheduleSvc.BLL.DTOs.WorkScheduleAssignmentDTOs;
using WorkScheduleSvc.BLL.Interfaces.Querys;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.BLL.Implements.QueryServices
{
    public class WorkScheduleAssignmentQueryService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        WorkScheduleAssignmentDetailsQueryHelper assignmentDetailsQueryHelper,
        ICurrentUserService currentUserService)
        : IWorkScheduleAssignmentQueryService
    {
        private readonly IGenericRepository<WorkScheduleAssignment> _workScheduleAssignmentRepository = unitOfWork.Repository<WorkScheduleAssignment>();
        private readonly IMapper _mapper = mapper;
        private readonly WorkScheduleAssignmentDetailsQueryHelper _assignmentDetailsQueryHelper = assignmentDetailsQueryHelper;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        #region Primary Methods
        public async Task<List<GetWorkScheduleAssignmentDTO>> GetAssignmentsAsync(int workScheduleId)
        {
            var assignments = await _workScheduleAssignmentRepository.GetAllAsync(x => x.WorkScheduleId == workScheduleId);
            return await BuildAssignmentDTOsAsync(assignments);
        }

        public async Task<List<GetWorkScheduleAssignmentDTO>> GetAssignmentsByWorkScheduleIdsAsync(List<int> workScheduleIds)
        {
            if (workScheduleIds.Count == 0)
            {
                return [];
            }

            var assignments = await _workScheduleAssignmentRepository.GetAllAsync(x => workScheduleIds.Contains(x.WorkScheduleId));
            return await BuildAssignmentDTOsAsync(assignments);
        }

        public async Task<List<GetWorkScheduleAssignmentDTO>> GetAssignmentsByStaffIdAsync(int staffId)
        {
            var assignments = await _workScheduleAssignmentRepository.GetAllAsync(x => x.StaffId == staffId);
            return await BuildAssignmentDTOsAsync(assignments);
        }

        public async Task<List<GetWorkScheduleAssignmentDTO>> GetAssignmentsByCurrentUserAsync()
        {
            return await GetAssignmentsByStaffIdAsync(_currentUserService.UserId);
        }

        public async Task<PaginationResult<GetWorkScheduleAssignmentDTO>> GetAllPaginatedAsync(FilterDTO filterDTO)
        {
            var (total, assignments) = await _workScheduleAssignmentRepository.GetAllPaginatedAsync(
                filterDTO.Filter,
                filterDTO.Sort,
                filterDTO.Page,
                filterDTO.PageSize);

            var assignmentDTOs = await BuildAssignmentDTOsAsync(assignments);
            return new PaginationResult<GetWorkScheduleAssignmentDTO>(total, filterDTO.PageSize, assignmentDTOs);
        }
        #endregion

        #region Helper Methods

        public async Task<List<GetWorkScheduleAssignmentDTO>> BuildAssignmentDTOsAsync(List<WorkScheduleAssignment> assignments)
        {
            var assignmentDTOs = _mapper.Map<List<GetWorkScheduleAssignmentDTO>>(assignments);
            await _assignmentDetailsQueryHelper.PopulateAssignmentDetailsAsync(assignmentDTOs);
            return assignmentDTOs;
        }
        #endregion
    }
}