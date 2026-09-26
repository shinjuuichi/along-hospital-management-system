using AutoMapper;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Services;
using SharedLibrary.Commons.Exceptions;
using WorkScheduleSvc.BLL.DTOs.TemplateDTOs.WorkScheduleTemplateDTOs;
using WorkScheduleSvc.BLL.Interfaces;
using WorkScheduleSvc.DAL.Models;

namespace WorkScheduleSvc.BLL.Implements
{
    public class WorkScheduleTemplateService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : BaseService<WorkScheduleTemplate,
             CreateWorkScheduleTemplateDTO,
             UpdateWorkScheduleTemplateDTO,
             GetWorkScheduleTemplateDTO>(unitOfWork, mapper,
                includes:
                [
                    nameof(WorkScheduleTemplate.WorkScheduleTemplateDayShifts),
                    nameof(WorkScheduleTemplate.WorkScheduleTemplateAssignmentForStaffRooms),
                    nameof(WorkScheduleTemplate.WorkScheduleTemplateAssignmentForStaffTeleRooms),
                ]),
                IWorkScheduleTemplateService
    {
        public override async Task<List<GetWorkScheduleTemplateDTO>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync(x => x.IsActive, _includes);
            return _mapper.Map<List<GetWorkScheduleTemplateDTO>>(entities);
        }

        public async Task DuplicateAsync(int id)
        {
            var template = await _repository.GetByIdAsync(id, includes: _includes)
                ?? throw new DataNotFoundException(typeof(WorkScheduleTemplate), id);

            var clone = _mapper.Map<WorkScheduleTemplate>(template);
            clone.Name = $"{template.Name} (Copy)";

            await _repository.AddAsync(clone);
            await _unitOfWork.SaveChangeAsync();
        }
    }
}
