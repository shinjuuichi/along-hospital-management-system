using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Base.Services;
using SharedLibrary.Commons.Filters;
using SharedLibrary.Commons.Results;

namespace SharedLibrary.Base.Controllers
{
    public abstract class GetController<TGetDTO>(IBaseGetService<TGetDTO> _service)
        : GetController<TGetDTO, FilterDTO>(_service)
        where TGetDTO : class;

    public abstract class GetController<TGetDTO, TFilter>(
        IBaseGetService<TGetDTO> _service)
        : BaseController
        where TGetDTO : class
        where TFilter : FilterDTO, new()
    {
        [HttpGet]
        public virtual async Task<IActionResult> GetAllPaginated(TFilter filterDTO)
        {
            var result = await _service.GetAllPaginatedAsync(filterDTO);
            return Result.SuccessData(result);
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return Result.SuccessData(result);
        }

        [HttpGet("all")]
        public virtual async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Result.SuccessData(result);
        }
    }
}
