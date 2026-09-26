using SharedLibrary.Commons.Filters;
using SharedLibrary.Commons.Results;

namespace SharedLibrary.Base.Services
{
    public interface IBaseGetService<TGetDTO>
    {
        Task<List<TGetDTO>> GetAllAsync();
        Task<PaginationResult<TGetDTO>> GetAllPaginatedAsync(FilterDTO filterDTO);
        Task<List<TGetDTO>> GetAllByIdsAsync(List<int> ids);
        Task<TGetDTO> GetByIdAsync(int id);
    }
}