using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Services;
using SharedLibrary.Commons.Filters;
using SharedLibrary.Commons.Results;

namespace SharedLibrary.Base.Controllers
{

    public abstract class CrudController<TCreateDTO, TUpdateDTO, TGetDTO>(
      IBaseCrudService<TCreateDTO, TUpdateDTO, TGetDTO> _service)
        : CrudController<TCreateDTO, TUpdateDTO, TGetDTO, FilterDTO>(_service)
      where TCreateDTO : class
      where TUpdateDTO : class
      where TGetDTO : class;

    public abstract class CrudController<TCreateDTO, TUpdateDTO, TGetDTO, TFilter>(
        IBaseCrudService<TCreateDTO, TUpdateDTO, TGetDTO> _service)
        : GetController<TGetDTO, TFilter>(_service)
        where TCreateDTO : class
        where TUpdateDTO : class
        where TGetDTO : class
        where TFilter : FilterDTO, new()
    {
        protected abstract string? EntityName { get; }

        [HttpPost]
        public virtual async Task<IActionResult> Create(TCreateDTO createDTO)
        {
            var result = await _service.CreateAsync(createDTO);
            return Result.SuccessData(result, $"{EntityName} created successfully");
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Update(int id, TUpdateDTO updateDTO)
        {
            var result = await _service.UpdateAsync(id, updateDTO);
            return Result.SuccessData(result, $"{EntityName} updated successfully");
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return Result.SuccessAction($"{EntityName} deleted successfully");
        }

        [HttpDelete("selected")]
        public virtual async Task<IActionResult> DeleteSelectedIds([FromQuery] List<int> ids)
        {
            await _service.DeleteSelectedIdsAsync(ids);
            return Result.SuccessAction($"{ids.Count} selected {EntityName}s deleted successfully");
        }
    }
}