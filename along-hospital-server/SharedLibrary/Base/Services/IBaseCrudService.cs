namespace SharedLibrary.Base.Services
{
    public interface IBaseCrudService<TCreateDTO, TUpdateDTO, TGetDTO> : IBaseGetService<TGetDTO>
    {
        Task<TGetDTO> CreateAsync(TCreateDTO createDTO);
        Task<TGetDTO> UpdateAsync(int id, TUpdateDTO updateDTO);
        Task DeleteAsync(int id);
        Task DeleteSelectedIdsAsync(List<int> ids);
    }
}
