using AutoMapper;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Services;
using SharedLibrary.Commons.Exceptions;
using SharedLibrary.Commons.Filters;
using SharedLibrary.Commons.Results;
using SharedLibrary.Enums;
using WorkScheduleSvc.BLL.DTOs.WorkScheduleAssignmentDTOs;
using WorkScheduleSvc.BLL.Interfaces;
using WorkScheduleSvc.BLL.Interfaces.Gateways;
using WorkScheduleSvc.BLL.Interfaces.Querys;
using WorkScheduleSvc.DAL.Enums;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.BLL.Implements
{
    public class WorkScheduleAssignmentService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IWorkScheduleAssignmentQueryService workScheduleAssignmentQueryService,
        IWorkScheduleAssignmentGateway workScheduleAssignmentGateway,
        IWorkScheduleAssignmentValidator workScheduleAssignmentValidator)
        : BaseService<WorkScheduleAssignment, CreateWorkScheduleAssignmentDTO, UpdateWorkScheduleAssignmentDTO, GetWorkScheduleAssignmentDTO>(
            unitOfWork,
            mapper),
          IWorkScheduleAssignmentService
    {
        private readonly IWorkScheduleAssignmentQueryService _workScheduleAssignmentQueryService = workScheduleAssignmentQueryService;
        private readonly IWorkScheduleAssignmentGateway _workScheduleAssignmentGateway = workScheduleAssignmentGateway;
        private readonly IWorkScheduleAssignmentValidator _workScheduleAssignmentValidator = workScheduleAssignmentValidator;

        #region Primary Methods
        public async Task<List<GetWorkScheduleAssignmentDTO>> BulkCreateAsync(CreateWorkScheduleAssignmentDTO dto)
        {
            _workScheduleAssignmentValidator.ValidateStaffIds(dto.StaffIds);
            var locationType = _workScheduleAssignmentValidator.ValidateLocationType(
                dto.LocationType,
                nameof(CreateWorkScheduleAssignmentDTO.LocationType));
            await _workScheduleAssignmentValidator.ValidateWorkScheduleMutableAsync(dto.WorkScheduleId, "created");

            var distinctStaffIds = dto.StaffIds.Distinct().ToList();
            var staffById = (await _workScheduleAssignmentGateway.GetStaffsAsync(distinctStaffIds))
                .ToDictionary(x => x.UserId);

            var doctorSpecialtyMap = staffById
                .Where(x => string.Equals(x.Value.Role, nameof(RoleEnum.Doctor), StringComparison.OrdinalIgnoreCase))
                .ToDictionary(x => x.Key, x => x.Value.SpecialtyId);

            if (doctorSpecialtyMap.Count > 0)
            {
                var locationSpecialtyId = locationType switch
                {
                    LocationTypeEnum.Room => (await _workScheduleAssignmentGateway.GetRoomAsync(dto.LocationId)).SpecialtyId,
                    LocationTypeEnum.TeleRoom => (await _workScheduleAssignmentGateway.GetTeleRoomAsync(dto.LocationId)).SpecialtyId,
                    _ => throw new ValidationFailureException(nameof(CreateWorkScheduleAssignmentDTO.LocationType), "LocationType is invalid.")
                };

                var invalidDoctorIds = doctorSpecialtyMap
                    .Where(x => x.Value != locationSpecialtyId)
                    .Select(x => x.Key)
                    .ToList();

                if (invalidDoctorIds.Count > 0)
                {
                    throw new ValidationFailureException(
                        "StaffIds",
                        $"Doctor [{string.Join(", ", invalidDoctorIds)}] must belong to the same specialty as {locationType} '{dto.LocationId}'.");
                }
            }
            var existingAssignments = await _repository.GetAllAsync(x =>
                x.WorkScheduleId == dto.WorkScheduleId && distinctStaffIds.Contains(x.StaffId));

            var existingStaffIds = existingAssignments.Select(x => x.StaffId).ToHashSet();
            var newStaffIds = distinctStaffIds.Where(x => !existingStaffIds.Contains(x)).ToList();
            if (newStaffIds.Count == 0)
            {
                return [];
            }

            var entities = newStaffIds.Select(staffId => new WorkScheduleAssignment
            {
                WorkScheduleId = dto.WorkScheduleId,
                StaffId = staffId,
                LocationId = dto.LocationId,
                LocationType = locationType
            }).ToList();

            await _repository.AddRangeAsync(entities);
            await _unitOfWork.SaveChangeAsync();

            return await _workScheduleAssignmentQueryService.BuildAssignmentDTOsAsync(entities);
        }
        #endregion

        #region Override Methods
        public override async Task<GetWorkScheduleAssignmentDTO> UpdateAsync(int id, UpdateWorkScheduleAssignmentDTO updateDTO)
        {
            var locationType = _workScheduleAssignmentValidator.ValidateLocationType(
                updateDTO.LocationType,
                nameof(UpdateWorkScheduleAssignmentDTO.LocationType));

            var entity = await _repository.GetByIdAsync(id)
                 ?? throw new DataNotFoundException(typeof(WorkScheduleAssignment), id);
            await _workScheduleAssignmentValidator.ValidateWorkScheduleMutableAsync(entity.WorkScheduleId, "updated");

            var staffById = (await _workScheduleAssignmentGateway.GetStaffsAsync([updateDTO.StaffId]))
                .ToDictionary(x => x.UserId);

            if (!staffById.TryGetValue(updateDTO.StaffId, out var staff))
            {
                throw new DataNotFoundException($"Staff not found for IDs: [{updateDTO.StaffId}].");
            }

            if (string.Equals(staff.Role, nameof(RoleEnum.Doctor), StringComparison.OrdinalIgnoreCase))
            {
                var locationSpecialtyId = locationType switch
                {
                    LocationTypeEnum.Room => (await _workScheduleAssignmentGateway.GetRoomAsync(updateDTO.LocationId)).SpecialtyId,
                    LocationTypeEnum.TeleRoom => (await _workScheduleAssignmentGateway.GetTeleRoomAsync(updateDTO.LocationId)).SpecialtyId,
                    _ => throw new ValidationFailureException(nameof(UpdateWorkScheduleAssignmentDTO.LocationType), "LocationType is invalid.")
                };

                if (staff.SpecialtyId != locationSpecialtyId)
                {
                    throw new ValidationFailureException(
                        "StaffId",
                        $"Doctor '{updateDTO.StaffId}' must belong to the same specialty as {locationType} '{updateDTO.LocationId}'.");
                }
            }

            _mapper.Map(updateDTO, entity);
            entity.LocationType = locationType;

            var result = _repository.Update(entity);
            await _unitOfWork.SaveChangeAsync();

            var assignmentDTOs = await _workScheduleAssignmentQueryService.BuildAssignmentDTOsAsync([result]);
            return assignmentDTOs[0];
        }

        public override async Task DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id)
                 ?? throw new DataNotFoundException(typeof(WorkScheduleAssignment), id);
            await _workScheduleAssignmentValidator.ValidateWorkScheduleMutableAsync(entity.WorkScheduleId, "deleted");

            _repository.Remove(entity);
            await _unitOfWork.SaveChangeAsync();
        }

        public override async Task<PaginationResult<GetWorkScheduleAssignmentDTO>> GetAllPaginatedAsync(FilterDTO filterDTO)
        {
            return await _workScheduleAssignmentQueryService.GetAllPaginatedAsync(filterDTO);
        }
        #endregion
    }
}
